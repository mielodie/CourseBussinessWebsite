using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.ReturnData.CourseData;
using CourseBusinessWebsite.Payloads.ReturnData.UserData;

namespace CourseBusinessWebsite.Payloads.ReturnData.AffiliateLinkData
{
    public class AffiliateLinkDTO : BaseDTO
    {
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public string Link { get; set; }
        public int ClickCount { get; set; }
        public DateTime CreateTime { get; set; }
        public UserDTO UserDTO { get; set; }
        public CourseDTO CourseDTO { get; set; }
    }
}
