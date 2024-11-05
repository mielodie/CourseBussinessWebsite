namespace CourseBusinessWebsite.Payloads.RequestData.UserRequest
{
    public class RequestUpdateUserInformation
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string Avatar{ get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
