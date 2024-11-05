using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Handle.EmailHandle;
using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.Converters;
using CourseBusinessWebsite.Payloads.RequestData.TokenRequest;
using CourseBusinessWebsite.Payloads.RequestData.UserRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.TokenData;
using CourseBusinessWebsite.Payloads.ReturnData.UserData;
using CourseBusinessWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CourseBusinessWebsite.Services.Implements
{
    public class AuthService : IAuthService
    {
        public readonly AppDbContext _context;
        private readonly ResponseObject<UserDTO> _responseObjectUserDTO;
        private readonly UserConverter _userConverter;
        private readonly ResponseObject<TokenDTO> _responseObjectTokenDTO;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(ResponseObject<UserDTO> responseObjectUserDTO, UserConverter userConverter, ResponseObject<TokenDTO> responseObjectTokenDTO, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _responseObjectUserDTO = responseObjectUserDTO;
            _userConverter = userConverter;
            _responseObjectTokenDTO = responseObjectTokenDTO;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _context = new AppDbContext();
        }
        public async Task<ResponseObject<UserDTO>> ChangeDecentralization(RequestChangeDecentralization request)
        {
            var user = await _context.users.SingleOrDefaultAsync(x => x.ID == request.UserID);
            if (user is null)
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy id người dùng", null);
            }
            var currentUser = _httpContextAccessor.HttpContext.User;

            if (!currentUser.Identity.IsAuthenticated)
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status401Unauthorized, "Người dùng không được xác thực hoặc không được xác định", null);
            }

            if (!currentUser.IsInRole("Admin"))
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status403Forbidden, "Người dùng không có quyền sử dụng chức năng này", null);
            }
            user.RoleID = request.RoleID;
            _context.users.Update(user);
            await _context.SaveChangesAsync();
            return _responseObjectUserDTO.ResponseSuccess("Thay đổi quyền người dùng thành công", _userConverter.EntityToDTO(user));
        }

        public async Task<ResponseObject<UserDTO>> ChangePassword(int userId, RequestChangePassword request)
        {
            var user = await _context.users.SingleOrDefaultAsync(x => x.ID == userId);

            bool checkOldPass = BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password);
            if (!checkOldPass)
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status400BadRequest, "Mật khẩu không chính xác", null);
            }
            if (!request.NewPassword.Equals(request.ConfirmNewPassword))
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status400BadRequest, "Mật khẩu không trùng khớp", null);
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            _context.users.Update(user);
            await _context.SaveChangesAsync();
            return _responseObjectUserDTO.ResponseSuccess("Đổi mật khẩu thành công", _userConverter.EntityToDTO(user));
        }

        public async Task<ResponseObject<UserDTO>> ConfirmCreateNewAccount(RequestConfirmCreateAccount request)
        {
            ConfirmEmail confirmEmail = _context.confirmEmails.Where(x => x.ConfirmationCode.Equals(request.ConfirmCode)).SingleOrDefault();
            if (confirmEmail == null)
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status400BadRequest, "Xác nhận đăng ký tài khoản thất bại", null);
            }
            if (confirmEmail.ExpirationTime < DateTime.Now)
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status400BadRequest, "Mã xác nhận đã hết hạn", null);
            }
            User user = await _context.users.FirstOrDefaultAsync(x => x.ID == confirmEmail.UserID);
            user.UserStatusID = 1;
            _context.confirmEmails.Remove(confirmEmail);
            _context.users.Update(user);
            await _context.SaveChangesAsync();
            return _responseObjectUserDTO.ResponseSuccess("Xác nhận đăng ký tài khoản thành công", _userConverter.EntityToDTO(user));
        }

        public async Task<string> ConfirmCreateNewPassword(RequestConfirmCreateNewPassword request)
        {
            ConfirmEmail confirm = await _context.confirmEmails.SingleOrDefaultAsync(x => x.ConfirmationCode.Equals(request.ConfirmCode));
            if (confirm is null)
            {
                return "Mã xác nhận không chính xác";
            }
            if (confirm.ExpirationTime < DateTime.Now)
            {
                return "Mã xác nhận đã hết hạn";
            }
            User user = await _context.users.SingleOrDefaultAsync(x => x.ID == confirm.UserID);
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            _context.users.Update(user);
            await _context.SaveChangesAsync();
            return "Tạo mật khẩu mới thành công";
        }

        public async Task<string> ForgotPassword(RequestForgotPassword request)
        {
            User user = await _context.users.FirstOrDefaultAsync(x => x.Email.Equals(request.Email));
            if (user is null)
            {
                return "Email không tồn tại trong hệ thống";
            }
            else
            {
                var confirms = _context.confirmEmails.Where(x => x.UserID == user.ID).ToList();
                _context.confirmEmails.RemoveRange(confirms);
                await _context.SaveChangesAsync();
                ConfirmEmail confirmEmail = new ConfirmEmail
                {
                    UserID = user.ID,
                    IsConfirmed = false,
                    ExpirationTime = DateTime.Now.AddHours(4),
                    ConfirmationCode = "MyNTD" + "@" + GenerateCodeActive().ToString()
                };
                await _context.confirmEmails.AddAsync(confirmEmail);
                await _context.SaveChangesAsync();
                string message = SendEmail(new EmailTo
                {
                    Mail = request.Email,
                    Subject = "Nhận mã xác nhận để tạo mật khẩu mới từ đây: ",
                    Content = $"Mã kích hoạt của bạn là: {confirmEmail.ConfirmationCode}, mã này sẽ hết hạn sau 4 tiếng"
                });
                return "Gửi mã xác nhận về email thành công, vui lòng kiểm tra email";
            }
        }

        public TokenDTO GenerateAccessToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value!);
            var role = _context.roles.SingleOrDefault(x => x.ID == user.RoleID);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim("Id", user.ID.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("Name", user.FullName),
                    new Claim("Username", user.Username),
                    new Claim(ClaimTypes.Role, role?.RoleName ?? "")
                }),
                Expires = DateTime.Now.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            RefreshToken rf = new RefreshToken
            {
                Token = refreshToken,
                ExpirationTime = DateTime.Now.AddHours(10),
                UserID = user.ID
            };

            _context.refreshTokens.Add(rf);
            _context.SaveChanges();

            TokenDTO data = new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserDTO = _userConverter.EntityToDTO(user)
            };
            return data;
        }

        public string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        public async Task<PageResult<UserDTO>> GetAllUsers(RequestUserInput input, int pageSize, int pageNumber)
        {
            var query = await _context.users.AsNoTracking().Where(x => x.IsActive == true).ToListAsync();
            if (!string.IsNullOrEmpty(input.Name))
            {
                query = query.Where(x => x.Username.ToLower().Contains(input.Name.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(input.Email))
            {
                query = query.Where(x => x.Email.ToLower().Contains(input.Email.ToLower())).ToList();
            }
            if (input.RoleID.HasValue)
            {
                query = query.Where(x => x.RoleID == input.RoleID).ToList();
            }
            var result = query.Select(x => _userConverter.EntityToDTO(x)).AsQueryable();
            var data = Pagination.GetPagedData(result, pageSize, pageNumber);
            return data;
        }

        public async Task<PageResult<UserDTO>> GetUserByName(string name, int pageSize, int pageNumber)
        {
            var query = _context.users.Where(x => x.FullName.Equals(name)).Select(x => _userConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<ResponseObject<TokenDTO>> Login(RequestLogin request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return _responseObjectTokenDTO.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);
            }
            var user = await _context.users.SingleOrDefaultAsync(x => x.Username.Equals(request.Username));
            if (user == null)
            {
                return _responseObjectTokenDTO.ResponseError(StatusCodes.Status404NotFound, "Người dùng không tồn tại", null);
            }
            if (user.UserStatusID == 2)
            {
                return _responseObjectTokenDTO.ResponseError(StatusCodes.Status401Unauthorized, "Tài khoản của bạn vẫn chưa được kích hoạt, vui lòng kích hoạt tài khoản", null);
            }
            bool checkPass = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!checkPass)
            {
                return _responseObjectTokenDTO.ResponseError(StatusCodes.Status400BadRequest, "Mật khẩu không chính xác", null);
            }
            if (user.IsActive == false)
            {
                return _responseObjectTokenDTO.ResponseError(StatusCodes.Status400BadRequest, "Tài khoản đã bị xóa, vui lòng thử lại", null);
            }
            else
            {
                return _responseObjectTokenDTO.ResponseSuccess("Đăng nhập tài khoản thành công", GenerateAccessToken(user));
            }
        }

        public async Task<ResponseObject<UserDTO>> Register(RequestRegister request)
        {
            if (string.IsNullOrWhiteSpace(request.Username)
                || string.IsNullOrWhiteSpace(request.Password)
                || string.IsNullOrWhiteSpace(request.PhoneNumber)
                || string.IsNullOrWhiteSpace(request.Email))
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);
            }
            if (!Validate.IsValidEmail(request.Email))
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status400BadRequest, "Định dạng email không hợp lệ", null);
            }
            if (!Validate.IsValidPhoneNumber(request.PhoneNumber))
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status400BadRequest, "Định dạng số điện thoại không hợp lệ", null);
            }
            if (_context.users.Any(x => x.Username.Equals(request.Username)))
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status400BadRequest, "Tên tài khoản đã tồn tại trên hệ thống", null);
            }
            if (_context.users.Any(x => x.Email.Equals(request.Email)))
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status400BadRequest, "Email đã tồn tại trên hệ thống", null);
            }
            if (_context.users.Any(x => x.PhoneNumber.Equals(request.PhoneNumber)))
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status400BadRequest, "Số điện thoại đã tồn tại trên hệ thống", null);
            }
            try
            {
                User user = new User();
                user.Email = request.Email;
                user.PhoneNumber = request.PhoneNumber;
                user.Username = request.Username;
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                user.FullName = request.FullName;
                user.RoleID = 3;
                user.UserStatusID = 2;
                user.IsActive = false;

                await _context.users.AddAsync(user);
                await _context.SaveChangesAsync();
                ConfirmEmail confirmEmail = new ConfirmEmail
                {
                    UserID = user.ID,
                    IsConfirmed = false,
                    ExpirationTime = DateTime.Now.AddHours(24),
                    ConfirmationCode = "MyNTD" + "@" + GenerateCodeActive().ToString(),
                    //RequiredDateTime = DateTime.Now
                };
                await _context.confirmEmails.AddAsync(confirmEmail);
                await _context.SaveChangesAsync();
                string message = SendEmail(new EmailTo
                {
                    Mail = request.Email,
                    Subject = "Nhận mã xác nhận để xác nhận đăng ký tài khoản mới từ đây: ",
                    Content = $"Mã kích hoạt của bạn là: {confirmEmail.ConfirmationCode}, mã này có hiệu lực là 24 tiếng"
                });
                return _responseObjectUserDTO.ResponseSuccess("Đăng ký tài khoản thành công, nhận mã xác nhận gửi về email để đăng ký tài khoản", _userConverter.EntityToDTO(user));

            }
            catch (Exception ex)
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }
        public string SendEmail(EmailTo emailTo)
        {
            if (!Validate.IsValidEmail(emailTo.Mail))
            {
                return "Định dạng email không hợp lệ";
            }
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("zzznguyenmy@gmail.com", "jvztzxbtyugsiaea"),
                EnableSsl = true
            };
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress("zzznguyenmy@gmail.com");
                message.To.Add(emailTo.Mail);
                message.Subject = emailTo.Subject;
                message.Body = emailTo.Content;
                message.IsBodyHtml = true;
                smtpClient.Send(message);

                return "Xác nhận gửi email thành công, lấy mã để xác thực";
            }
            catch (Exception ex)
            {
                return "Lỗi khi gửi email: " + ex.Message;
            }
        }
        private int GenerateCodeActive()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }

        public ResponseObject<TokenDTO> RenewAccessToken(RequestRenewToken request)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var secretKey = _configuration.GetSection("AppSettings:SecretKey").Value;

                var tokenValidation = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey))
                };

                var tokenAuthentication = jwtTokenHandler.ValidateToken(request.AccessToken, tokenValidation, out var validatedToken);
                if (!(validatedToken is JwtSecurityToken jwtSecurityToken) || jwtSecurityToken.Header.Alg != SecurityAlgorithms.HmacSha256)
                {
                    return _responseObjectTokenDTO.ResponseError(StatusCodes.Status400BadRequest, "Token không hợp lệ", null);
                }
                var refreshToken = _context.refreshTokens.SingleOrDefault(x => x.Token.Equals(request.RefreshToken));
                if (refreshToken == null)
                {
                    return _responseObjectTokenDTO.ResponseError(StatusCodes.Status404NotFound, "RefreshToken không tồn tại trong database", null);
                }
                if (refreshToken.ExpirationTime < DateTime.Now)
                {
                    return _responseObjectTokenDTO.ResponseError(StatusCodes.Status401Unauthorized, "RefreshToken đã hết hạn", null);
                }
                var user = _context.users.SingleOrDefault(x => x.ID == refreshToken.UserID);
                if (user == null)
                {
                    return _responseObjectTokenDTO.ResponseError(StatusCodes.Status404NotFound, "Người dùng không tồn tại", null);
                }
                var newToken = GenerateAccessToken(user);
                return _responseObjectTokenDTO.ResponseSuccess("Token đã được làm mới thành công", newToken);
            }
            catch (SecurityTokenValidationException ex)
            {
                return _responseObjectTokenDTO.ResponseError(StatusCodes.Status400BadRequest, "Lỗi xác thực token: " + ex.Message, null);
            }
            catch (Exception ex)
            {
                return _responseObjectTokenDTO.ResponseError(StatusCodes.Status500InternalServerError, "Lỗi không xác định: " + ex.Message, null);
            }
        }

        public async Task<ResponseObject<UserDTO>> UpdateUserInformation(int userId, RequestUpdateUserInformation request)
        {
            var user = await _context.users.SingleOrDefaultAsync(x => x.ID == userId);

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.PhoneNumber) || string.IsNullOrWhiteSpace(request.Email))
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);
            }
            if (!Validate.IsValidEmail(request.Email))
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status400BadRequest, "Định dạng email không hợp lệ", null);
            }
            if (!Validate.IsValidEmail(request.PhoneNumber))
            {
                return _responseObjectUserDTO.ResponseError(StatusCodes.Status400BadRequest, "Định dạng số điện thoại không hợp lệ", null);
            }
            user.Username = request.Username;
            user.FullName = request.Name;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;
            user.Avatar = request.Avatar;
            user.DateOfBirth = request.DateOfBirth;
            user.Sex = request.Sex;
            _context.users.Update(user);
            await _context.SaveChangesAsync();
            return _responseObjectUserDTO.ResponseSuccess("Cập nhật thông tin thành công", _userConverter.EntityToDTO(user));
        }
    }
}
