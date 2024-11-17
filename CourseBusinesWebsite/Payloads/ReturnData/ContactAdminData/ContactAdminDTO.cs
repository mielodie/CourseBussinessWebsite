namespace CourseBusinessWebsite.Payloads.ReturnData.ContactAdminData
{
    public class ContactAdminDTO : BaseDTO
    {
        public string ContactPersonName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ContactAt { get; set; }
        public bool IsContacted { get; set; }
    }
}
