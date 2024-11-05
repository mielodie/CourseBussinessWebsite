namespace CourseBusinessWebsite.Payloads.ReturnData.UserData
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNunber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Avatar { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public int NumberOfViolations { get; set; }
        public double TotalMoney { get; set; }
    }
}
