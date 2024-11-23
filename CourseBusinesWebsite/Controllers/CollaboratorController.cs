using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.RequestData.AffiliateLinkRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.AffiliateLinkData;
using CourseBusinessWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseBusinessWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        private readonly IAffiliateLinkService _iAffiliateLinkService;

        public CollaboratorController(IAffiliateLinkService iAffiliateLinkService)
        {
            _iAffiliateLinkService = iAffiliateLinkService;
        }
        [HttpPost("affiliateLink/CreateAffiliateLink")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateAffiliateLink([FromBody]RequestCreateAffiliateLink request)
        {
            int userID = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await _iAffiliateLinkService.CreateAffiliateLink(userID, request));
        }
        [HttpGet("affiliateLink/GetByID")]
        public async Task<IActionResult> GetByID([FromRoute] int affiliateLinkID)
        {
            return Ok(await _iAffiliateLinkService.GetByID(affiliateLinkID));
        }
        [HttpGet("affiliateLink/GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            return Ok(await _iAffiliateLinkService.GetAll(pageSize, pageNumber));
        }
    }
}
