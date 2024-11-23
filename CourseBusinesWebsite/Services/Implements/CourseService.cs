using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Handle.ImageHandle;
using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.Converters;
using CourseBusinessWebsite.Payloads.RequestData.CourseRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.CourseData;
using CourseBusinessWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseBusinessWebsite.Services.Implements
{
    public class CourseService : ICourseService
    {
        private readonly ResponseObject<CourseDTO> _responseObjectCourseDTO;
        private readonly CourseConverter _courseConverter;
        private readonly AppDbContext _context;

        public CourseService(ResponseObject<CourseDTO> responseObjectCourseDTO, CourseConverter courseConverter)
        {
            _responseObjectCourseDTO = responseObjectCourseDTO;
            _courseConverter = courseConverter;
            _context = new AppDbContext();
        }

        public async Task<ResponseObject<CourseDTO>> CreateCourse(int userID, RequestCreateCourse request)
        {
            Category category = await _context.categories.SingleOrDefaultAsync(x => x.ID == request.CategoryID);
            if(category == null)
            {
                return _responseObjectCourseDTO.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy loại khóa học này", null);
            }
            Course course = new Course
            {
                AvatarCourse = await UploadImage.Upfile(request.AvatarCourse),
                CategoryID = request.CategoryID,
                Creator = _context.users.SingleOrDefaultAsync(x => x.ID == userID).Result.FullName,
                Price = request.Price,
                Duration = request.Duration,
                Description = request.Description,
                Title = request.Title,
            };
            await _context.courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return _responseObjectCourseDTO.ResponseSuccess("Thêm khóa học thành công", _courseConverter.EntityToDTO(course));
        }

        public async Task<PageResult<CourseDTO>> GetAllCourse(Filter filter, int pageSize, int pageNumber)
        {
            var listCourse = _context.courses.Include(x => x.Category).AsNoTracking().Where(x => x.isActive == true).AsQueryable();
            if (filter.CategoryID.HasValue)
            {
                listCourse = listCourse.Where(x => x.CategoryID == filter.CategoryID);
            }
            if (!string.IsNullOrEmpty(filter.Title))
            {
                listCourse = listCourse.Where(x => x.Title.Trim().ToLower().Contains(filter.Title.Trim().ToLower()));
            }
            var result = listCourse.Select(x => _courseConverter.EntityToDTO(x));
            var listPage = Pagination.GetPagedData(result, pageSize, pageNumber);
            return listPage;
        }

        public async Task<ResponseObject<CourseDTO>> GetCourseByID(int courseID)
        {
            Course course = await _context.courses.SingleOrDefaultAsync(x => x.ID == courseID && x.isActive == true);
            return _responseObjectCourseDTO.ResponseSuccess("Lấy thông tin khóa học thành công", _courseConverter.EntityToDTO(course));
        }

        public async Task<string> RemoveCourse(int courseID)
        {
            Course course = await _context.courses.SingleOrDefaultAsync(x => x.ID == courseID);
            if (course == null)
            {
                return "Không tìm thấy khóa học này";
            }
            course.isActive = false;
            _context.courses.Update(course);
            await _context.SaveChangesAsync();
            return "Xóa khóa học thành công";
        }

        public async Task<ResponseObject<CourseDTO>> UpdateCourse(RequestUpdateCourse request)
        {
            Course course = await _context.courses.SingleOrDefaultAsync(x => x.ID == request.CourseID);
            if (course == null)
            {
                return _responseObjectCourseDTO.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy khóa học này", null);
            }
            course.Title = request.Title;
            course.Description = request.Description;
            course.Price = request.Price;
            course.AvatarCourse = await UploadImage.Upfile(request.AvatarCourse);
            course.Duration = request.Duration;
            course.CategoryID = request.CategoryID;
            course.Creator = request.Creator;
            _context.courses.Update(course);
            await _context.SaveChangesAsync();
            return _responseObjectCourseDTO.ResponseSuccess("Sửa thông tin khóa học thành công", _courseConverter.EntityToDTO(course));
        }
    }
}
