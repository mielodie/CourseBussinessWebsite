using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.Converters;
using CourseBusinessWebsite.Payloads.RequestData.UserCourseRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.UserCourseData;
using CourseBusinessWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseBusinessWebsite.Services.Implements
{
    public class UserCourseService : IUserCourseSevice
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<UserCourseDTO> _responseObjectUserCourseDTO;
        private readonly UserCourseConverter _converter;

        public UserCourseService(ResponseObject<UserCourseDTO> responseObjectUserCourseDTO, UserCourseConverter converter)
        {
            _context = new AppDbContext();
            _responseObjectUserCourseDTO = responseObjectUserCourseDTO;
            _converter = converter;
        }

        public async Task<ResponseObject<UserCourseDTO>> GetByID(int userCourseID)
        {
            UserCourse userCourse = await _context.userCourses.SingleOrDefaultAsync(x => x.ID == userCourseID);
            if (userCourse == null)
            {
                return _responseObjectUserCourseDTO.ResponseError(StatusCodes.Status404NotFound, "Khóa học này bạn chưa đăng ký", null);
            }
            return _responseObjectUserCourseDTO.ResponseSuccess("Khóa học này bạn chưa đăng ký", _converter.EntityToDTO(userCourse));
        }

        public async Task<PageResult<UserCourseDTO>> GetCourseByUserID(int userID, int pageSize, int pageNumber)
        {
            var listCourseUser = _context.userCourses.AsNoTracking().Where(x => x.UserID == userID).AsQueryable();
            var result = listCourseUser.Select(x => _converter.EntityToDTO(x));
            var listPage = Pagination.GetPagedData(result, pageSize, pageNumber);
            return listPage;
        }

        public async Task<ResponseObject<UserCourseDTO>> RegisterCourse(RequestRegisterCourse request)
        {
            Course course = await _context.courses.FirstOrDefaultAsync(x => x.ID == request.CourseID);
            if (course == null)
            {
                return _responseObjectUserCourseDTO.ResponseError(StatusCodes.Status404NotFound, "Khóa học này không tồn tại", null);
            }
            User user = await _context.users.FirstOrDefaultAsync(x => x.ID == request.UserID);
            if (course == null)
            {
                return _responseObjectUserCourseDTO.ResponseError(StatusCodes.Status404NotFound, "Người dùng không tồn tại", null);
            }
            UserCourse registerCourse = new UserCourse
            {
                CourseID = request.CourseID,
                UserID = request.UserID,
                CourseRegistrationPeriod = DateTime.Now,
                PercentCompleted = 0,
                IsCompleted = false
            };
            await _context.userCourses.AddAsync(registerCourse);
            await _context.SaveChangesAsync();
            return _responseObjectUserCourseDTO.ResponseSuccess("Đăng kí khóa học thành công", _converter.EntityToDTO(registerCourse));
        }

        public Task<string> RemoveTheRegisteredCourse(int courseID)
        {
            throw new NotImplementedException();
        }
    }
}
