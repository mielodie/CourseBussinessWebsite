using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.ReturnData.LessonData;

namespace CourseBusinessWebsite.Payloads.Converters
{
    public class LessonConverter
    {
        public LessonDTO EntityToDTO(Lesson lesson)
        {
            return new LessonDTO
            {
                ID = lesson.ID,
                ContentURL = lesson.ContentURL,
                CreateAt = lesson.CreateAt,
                Duration = lesson.Duration, 
                LessonName = lesson.LessonName,
                Order = lesson.Order
            };
        }
    }
}
