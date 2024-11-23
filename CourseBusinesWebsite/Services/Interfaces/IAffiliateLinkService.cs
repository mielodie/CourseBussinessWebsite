using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.RequestData.AffiliateLinkRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.AffiliateLinkData;

namespace CourseBusinessWebsite.Services.Interfaces
{
    public interface IAffiliateLinkService
    {
        Task<ResponseObject<AffiliateLinkDTO>> CreateAffiliateLink(int usedID, RequestCreateAffiliateLink request);
        Task<ResponseObject<AffiliateLinkDTO>> GetByID(int affiliateLinkID);
        Task<PageResult<AffiliateLinkDTO>> GetAll(int pageSize, int pageNumber);
    }
}
