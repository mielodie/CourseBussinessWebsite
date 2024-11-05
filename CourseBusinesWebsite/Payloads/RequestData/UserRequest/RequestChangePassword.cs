namespace CourseBusinessWebsite.Payloads.RequestData.UserRequest
{
    public class RequestChangePassword
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
