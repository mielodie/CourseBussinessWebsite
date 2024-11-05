using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.RequestData.TokenRequest;
using CourseBusinessWebsite.Payloads.RequestData.UserRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.TokenData;
using CourseBusinessWebsite.Payloads.ReturnData.UserData;

namespace CourseBusinessWebsite.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseObject<UserDTO>> Register(RequestRegister request);
        Task<ResponseObject<TokenDTO>> Login(RequestLogin request);
        TokenDTO GenerateAccessToken(User user);
        ResponseObject<TokenDTO> RenewAccessToken(RequestRenewToken request);
        string GenerateRefreshToken();
        Task<ResponseObject<UserDTO>> ConfirmCreateNewAccount(RequestConfirmCreateAccount request);
        Task<ResponseObject<UserDTO>> ChangePassword(int userId, RequestChangePassword request);
        Task<string> ForgotPassword(RequestForgotPassword request);
        Task<string> ConfirmCreateNewPassword(RequestConfirmCreateNewPassword request);
        Task<ResponseObject<UserDTO>> ChangeDecentralization(RequestChangeDecentralization request);
        Task<ResponseObject<UserDTO>> UpdateUserInformation(int userId, RequestUpdateUserInformation request);
        Task<PageResult<UserDTO>> GetAllUsers(RequestUserInput input, int pageSize, int pageNumber);
        Task<PageResult<UserDTO>> GetUserByName(string name, int pageSize, int pageNumber);
    }
}
