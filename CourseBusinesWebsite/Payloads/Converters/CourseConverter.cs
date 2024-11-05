using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.ReturnData.CourseData;

namespace CourseBusinessWebsite.Payloads.Converters
{
    public class CourseConverter
    {
        private readonly AppDbContext _context;
        private readonly LessonConverter _converter;
        public CourseConverter()
        {
            _context = new AppDbContext();
            _converter = new LessonConverter();
        }
        public CourseDTO EntityToDTO(Course course)
        {
            return new CourseDTO
            {
                AvatarCourse = course.AvatarCourse,
                Creator = course.Creator,
                Description = course.Description,
                Duration = course.Duration,
                Price = course.Price,
                Title = course.Title,
                LessonDTOs = _context.lessons.Where(x => x.CourseID == course.ID).Select(x => _converter.EntityToDTO(x))
            };
        }
    }
}
