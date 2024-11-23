using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.RequestData.CourseRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.CourseData;

namespace CourseBusinessWebsite.Services.Interfaces
{
    public interface ICourseService
    {
        Task<ResponseObject<CourseDTO>> CreateCourse(int userID, RequestCreateCourse request);
        Task<ResponseObject<CourseDTO>> UpdateCourse(RequestUpdateCourse request);
        Task<string> RemoveCourse(int courseID);
        Task<PageResult<CourseDTO>> GetAllCourse(Filter filter, int pageSize, int pageNumber);
        Task<ResponseObject<CourseDTO>> GetCourseByID(int courseID);
    }
}
