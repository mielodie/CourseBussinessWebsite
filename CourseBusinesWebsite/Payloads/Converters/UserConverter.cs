using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.ReturnData.UserData;

namespace CourseBusinessWebsite.Payloads.Converters
{
    public class UserConverter
    {
        public UserDTO EntityToDTO(User user)
        {
            return new UserDTO
            {
                Email = user.Email,
                Address = user.Address,
                Avatar = user.Avatar,
                DateOfBirth = user.DateOfBirth,
                FullName = user.FullName,
                NumberOfViolations = user.NumberOfViolations,
                PhoneNunber = user.PhoneNumber,
                Sex = user.Sex,
                TotalMoney = user.TotalMoney,
                Username = user.Username
            };
        }
    }
}
