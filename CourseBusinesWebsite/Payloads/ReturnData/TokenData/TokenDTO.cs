using CourseBusinessWebsite.Payloads.ReturnData.UserData;

namespace CourseBusinessWebsite.Payloads.ReturnData.TokenData
{
    public class TokenDTO : BaseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public UserDTO UserDTO { get; set; }
    }
}
