using CloudinaryDotNet;
using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.Converters;
using CourseBusinessWebsite.Payloads.RequestData.LessonRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.LessonData;
using CourseBusinessWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseBusinessWebsite.Services.Implements
{
    public class LessonService : ILessonService
    {
        private readonly AppDbContext _context;
        private readonly LessonConverter _converter;
        private readonly ResponseObject<LessonDTO> _responseObjectLessonDTO;

        public LessonService(LessonConverter converter, ResponseObject<LessonDTO> responseObjectLessonDTO)
        {
            _context = new AppDbContext();
            _converter = converter;
            _responseObjectLessonDTO = responseObjectLessonDTO;
        }

        public async Task<ResponseObject<LessonDTO>> CreateLesson(RequestCreateLesson request)
        {
            Course course = await _context.courses.SingleOrDefaultAsync(x => x.ID == request.CourseID);
            if (course == null)
            {
                return _responseObjectLessonDTO.ResponseError(StatusCodes.Status404NotFound, "Khóa học này không tồn tại", null);
            }
            Lesson lesson = new Lesson();
            lesson.CreateAt = DateTime.Now;
            lesson.ContentURL = request.ContentURL;
            lesson.Duration = request.Duration;
            lesson.Order = request.Order;
            lesson.CourseID = request.CourseID;
            lesson.LessonName = request.LessonName;
            lesson.LessonStatusID = 1;
            await _context.lessons.AddAsync(lesson);
            await _context.SaveChangesAsync();
            return _responseObjectLessonDTO.ResponseSuccess("Thêm bài học vào khóa học '" + course.Title + "' thành công", _converter.EntityToDTO(lesson));
        }

        public async Task<PageResult<LessonDTO>> GetAll(int pageSize, int pageNumber)
        {
            var lessonList = _context.lessons.AsNoTracking().AsQueryable();
            var result = lessonList.Select(x => _converter.EntityToDTO(x));
            var pageList = Pagination.GetPagedData(result, pageSize, pageNumber);
            return pageList;
        }

        public async Task<PageResult<LessonDTO>> GetByCourseID(int courseID, int pageSize, int pageNumber)
        {
            var lessonList = _context.lessons.AsNoTracking().Where(x => x.CourseID == courseID).AsQueryable();
            var result = lessonList.Select(x => _converter.EntityToDTO(x));
            var pageList = Pagination.GetPagedData(result, pageSize, pageNumber);
            return pageList;
        }

        public async Task<ResponseObject<LessonDTO>> GetByID(int lessonID)
        {
            Lesson lesson = await _context.lessons.SingleOrDefaultAsync(x => x.ID == lessonID);
            return _responseObjectLessonDTO.ResponseSuccess("Bài học có ID = " + lessonID, _converter.EntityToDTO(lesson));
        }

        public async Task<string> RemoveLesson(int lessonID)
        {
            Lesson lesson = await _context.lessons.SingleOrDefaultAsync(x => x.ID == lessonID);
            if (lesson == null)
            {
                return "Bài học này không tồn tại";
            }
            _context.lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return "Xóa bài học thành công";
        }

        public async Task<string> UnblockLesson(int lessonID)
        {
            Lesson lesson = await _context.lessons.SingleOrDefaultAsync(x => x.ID == lessonID);
            if (lesson == null)
            {
                return "Bài học này không tồn tại";
            }
            if(lesson.LessonStatusID == 1)
            {
                lesson.LessonStatusID = 2;
                _context.lessons.Update(lesson);
                await _context.SaveChangesAsync();
                return "Mở bài học thành công";
            }
            lesson.LessonStatusID = 1;
            _context.lessons.Update(lesson);
            await _context.SaveChangesAsync();
            return "Khóa bài học thành công";
        }

        public async Task<ResponseObject<LessonDTO>> UpdateLesson(RequestUpdateLesson request)
        {
            Lesson lesson = await _context.lessons.SingleOrDefaultAsync(x => x.ID == request.LessonID);
            if (lesson == null)
            {
                return _responseObjectLessonDTO.ResponseError(StatusCodes.Status404NotFound, "Bài học này không tồn tại", null);
            }
            lesson.UpdateAt = DateTime.Now;
            lesson.ContentURL = request.ContentURL;
            lesson.Duration = request.Duration;
            lesson.Order = request.Order;
            lesson.CourseID = request.CourseID;
            lesson.LessonName = request.LessonName;
            _context.lessons.Update(lesson);
            await _context.SaveChangesAsync();
            return _responseObjectLessonDTO.ResponseSuccess("Sửa bài học thành công", _converter.EntityToDTO(lesson));
        }
    }
}
