namespace CourseBusinessWebsite.Payloads.ReturnData.UserCourseData
{
    public class UserCourseDTO : BaseDTO
    {
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public int PercentCompleted { get; set; }
        public DateTime CourseRegistrationPeriod { get; set; }
        public bool IsCompleted { get; set; }
    }
}
