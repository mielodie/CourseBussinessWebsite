using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.ReturnData.UserCourseData;

namespace CourseBusinessWebsite.Payloads.Converters
{
    public class UserCourseConverter
    {
        public UserCourseDTO EntityToDTO(UserCourse userCourse)
        {
            return new UserCourseDTO()
            {
                ID = userCourse.ID,
                CourseID = userCourse.CourseID,
                CourseRegistrationPeriod = userCourse.CourseRegistrationPeriod,
                IsCompleted = userCourse.IsCompleted,
                PercentCompleted = userCourse.PercentCompleted,
                UserID = userCourse.UserID
            };
        }
    }
}
