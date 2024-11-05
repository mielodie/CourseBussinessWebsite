using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.RequestData.UserCourseRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.UserCourseData;

namespace CourseBusinessWebsite.Services.Interfaces
{
    public interface IUserCourseSevice
    {
        Task<ResponseObject<UserCourseDTO>> RegisterCourse(RequestRegisterCourse request);
        Task<string> RemoveTheRegisteredCourse(int courseID);
        Task<PageResult<UserCourseDTO>> GetCourseByUserID(int userID, int pageSize, int pageNumber);
        Task<ResponseObject<UserCourseDTO>> GetByID(int userCourseID);
    }
}
