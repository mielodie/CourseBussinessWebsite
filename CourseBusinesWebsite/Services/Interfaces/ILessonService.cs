using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.RequestData.LessonRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.LessonData;

namespace CourseBusinessWebsite.Services.Interfaces
{
    public interface ILessonService
    {
        Task<ResponseObject<LessonDTO>> CreateLesson(RequestCreateLesson request);
        Task<ResponseObject<LessonDTO>> UpdateLesson(RequestUpdateLesson request);
        Task<string> RemoveLesson(int lessonID);
        Task<string> UnblockLesson(int lessonID);
        Task<PageResult<LessonDTO>> GetAll(int pageSize, int pageNumber);
        Task<PageResult<LessonDTO>> GetByCourseID(int courseID, int pageSize, int pageNumber);
        Task<ResponseObject<LessonDTO>> GetByID(int lessonID);
    }
}
