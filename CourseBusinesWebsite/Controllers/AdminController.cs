using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.RequestData.AffiliateLinkRequest;
using CourseBusinessWebsite.Payloads.RequestData.ContactAdminRequest;
using CourseBusinessWebsite.Payloads.RequestData.CourseRequest;
using CourseBusinessWebsite.Payloads.RequestData.LessonRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.CourseData;
using CourseBusinessWebsite.Payloads.ReturnData.LessonData;
using CourseBusinessWebsite.Payloads.ReturnData.RoleData;
using CourseBusinessWebsite.Services.Interfaces;
using ImageProcessor.Processors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Filter = CourseBusinessWebsite.Payloads.RequestData.CourseRequest.Filter;

namespace CourseBusinessWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IContactAdminService _iContactAdminService;
        private readonly ICourseService _iCourseService;
        private readonly ILessonService _iLessonService;
        private readonly IRoleService _iRoleService;

        public AdminController(IContactAdminService iContactAdminService, ICourseService iCourseService, ILessonService iLessonService, IRoleService iRoleService)
        {
            _iContactAdminService = iContactAdminService;
            _iCourseService = iCourseService;
            _iLessonService = iLessonService;
            _iRoleService = iRoleService;
        }
        #region contactAdmin
        [HttpPost("contactAdmin/CreateContact")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateContact([FromBody] RequestCreateContactAdmin request)
        {
            request.ContactPersonID = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await _iContactAdminService.CreateContact(request));
        }
        [HttpPut("contactAdmin/RemoveContact")]
        public async Task<IActionResult> RemoveContact([FromQuery] int contactAdminID)
        {
            return Ok(await _iContactAdminService.RemoveContact(contactAdminID));
        }
        [HttpGet("contactAdmin/GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            return Ok(await _iContactAdminService.GetAll(pageSize, pageNumber));
        }
        [HttpGet("contactAdmin/GetByID")]
        public async Task<IActionResult> GetByID([FromRoute] int contactAdminID)
        {
            return Ok(await _iContactAdminService.GetByID(contactAdminID));
        }
        #endregion
        #region course
        [HttpPost("course/CreateCourse")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateCourse([FromBody] RequestCreateCourse request)
        {
            int userID = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await _iCourseService.CreateCourse(userID, request));
        }
        [HttpPut("course/UpdateCourse")]
        public async Task<IActionResult> UpdateCourse([FromBody] RequestUpdateCourse request)
        {
            return Ok(await _iCourseService.UpdateCourse(request));
        }
        [HttpPut("course/RemoveCourse")]
        public async Task<IActionResult> RemoveCourse([FromQuery] int courseID)
        {
            return Ok(await _iCourseService.RemoveCourse(courseID));
        }
        [HttpGet("course/GetAll")]
        public async Task<IActionResult> GetAllCourse([FromQuery] Filter filter, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            return Ok(await _iCourseService.GetAllCourse(filter, pageSize, pageNumber));
        }
        [HttpGet("course/GetByID")]
        public async Task<IActionResult> GetCourseByID([FromRoute] int courseID)
        {
            return Ok(await _iCourseService.GetCourseByID(courseID));
        }
        #endregion
        #region lesson
        [HttpPost("lesson/CreateLesson")]
        public async Task<IActionResult> CreateLesson([FromBody] RequestCreateLesson request)
        {
            return Ok(await _iLessonService.CreateLesson(request));
        }
        [HttpPut("lesson/UpdateLesson")]
        public async Task<IActionResult> UpdateLesson([FromBody] RequestUpdateLesson request)
        {
            return Ok(await _iLessonService.UpdateLesson(request));
        }
        [HttpDelete("lesson/RemoveLesson")]
        public async Task<IActionResult> RemoveLesson([FromQuery] int lessonID)
        {
            return Ok(await _iLessonService.RemoveLesson(lessonID));
        }
        [HttpPut("lesson/UnblockLesson")]
        public async Task<IActionResult> UnblockLesson([FromRoute] int lessonID)
        {
            return Ok(await _iLessonService.UnblockLesson(lessonID));
        }
        [HttpGet("lesson/GetAll")]
        public async Task<IActionResult> GetAllLesson([FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            return Ok(await _iLessonService.GetAll(pageSize, pageNumber));
        }
        [HttpGet("lesson/GetByCourseID")]
        public async Task<IActionResult> GetByCourseID([FromQuery] int courseID, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            return Ok(await _iLessonService.GetByCourseID(courseID, pageSize, pageNumber));
        }
        [HttpGet("lesson/GetByID")]
        public async Task<IActionResult> GetLessonByID([FromRoute] int lessonID)
        {
            return Ok(await _iLessonService.GetByID(lessonID));
        }
        #endregion
        #region role
        [HttpPost("role/CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            return Ok(await _iRoleService.CreateRole(roleName));
        }
        [HttpPut("role/UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromQuery] int roleID, [FromQuery] string roleName)
        {
            return Ok(await _iRoleService.UpdateRole(roleID, roleName));
        }
        [HttpDelete("role/RemoveRole")]
        public async Task<IActionResult> RemoveRole([FromQuery] int roleID)
        {
            return Ok(await _iRoleService.RemoveRole(roleID));
        }
        [HttpGet("role/GetAll")]
        public async Task<IActionResult> GetAllRole([FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            return Ok(await _iRoleService.GetAll(pageSize, pageNumber));
        }
        [HttpGet("role/GetByID")]
        public async Task<IActionResult> GetRoleByID([FromRoute] int roleID)
        {
            return Ok(await _iRoleService.GetByID(roleID));
        }
        #endregion
    }
}
