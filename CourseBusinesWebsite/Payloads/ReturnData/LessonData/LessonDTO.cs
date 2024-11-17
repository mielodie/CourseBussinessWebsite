namespace CourseBusinessWebsite.Payloads.ReturnData.LessonData
{
    public class LessonDTO : BaseDTO
    {
        public string LessonName { get; set; }
        public string ContentURL { get; set; }
        public DateTime CreateAt { get; set; }
        public int Duration { get; set; }
        public int Order { get; set; }
    }
}
