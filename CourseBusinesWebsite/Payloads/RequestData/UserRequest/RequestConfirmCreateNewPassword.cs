namespace CourseBusinessWebsite.Payloads.RequestData.UserRequest
{
    public class RequestConfirmCreateNewPassword
    {
        public string ConfirmCode { get; set; }
        public string NewPassword { get; set; }
    }
}
