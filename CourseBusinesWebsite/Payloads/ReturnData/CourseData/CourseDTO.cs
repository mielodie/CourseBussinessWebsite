using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.ReturnData.LessonData;

namespace CourseBusinessWebsite.Payloads.ReturnData.CourseData
{
    public class CourseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string AvatarCourse { get; set; }
        public int Duration { get; set; }
        public string Creator { get; set; }
        public IQueryable<LessonDTO> LessonDTOs {  get; set; } 
    }
}
