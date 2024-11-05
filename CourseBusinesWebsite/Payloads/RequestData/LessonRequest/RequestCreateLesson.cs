namespace CourseBusinessWebsite.Payloads.RequestData.LessonRequest
{
    public class RequestCreateLesson
    {
        public int CourseID { get; set; }
        public string LessonName { get; set; }
        public string ContentURL { get; set; }
        public int Duration { get; set; }
        public int Order { get; set; }
    }
}
