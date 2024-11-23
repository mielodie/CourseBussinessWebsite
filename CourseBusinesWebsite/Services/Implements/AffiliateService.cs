using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.Converters;
using CourseBusinessWebsite.Payloads.RequestData.AffiliateLinkRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.AffiliateLinkData;
using CourseBusinessWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseBusinessWebsite.Services.Implements
{
    public class AffiliateService : IAffiliateLinkService
    {
        private readonly ResponseObject<AffiliateLinkDTO> _responseObjectAffiliateLinkDTO;
        private readonly AffiliateConverter _converter;
        private readonly AppDbContext _context;

        public AffiliateService(ResponseObject<AffiliateLinkDTO> responseObjectAffiliateLinkDTO, AffiliateConverter converter)
        {
            _context = new AppDbContext();
            _responseObjectAffiliateLinkDTO = responseObjectAffiliateLinkDTO;
            _converter = converter;
        }

        public async Task<ResponseObject<AffiliateLinkDTO>> CreateAffiliateLink(int usedID, RequestCreateAffiliateLink request)
        {
            Course course = await _context.courses.FirstOrDefaultAsync(x => x.ID == request.CourseID);
            if(course is null)
            {
                return _responseObjectAffiliateLinkDTO.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy khóa học", null);
            }
            AffiliateLink affiliateLink = new AffiliateLink();
            affiliateLink.CreateTime = DateTime.Now;
            affiliateLink.Link = request.Link;
            affiliateLink.CourseID = request.CourseID;

            await _context.courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return _responseObjectAffiliateLinkDTO.ResponseSuccess("Share link thành công", _converter.EntityToDTO(affiliateLink));
        }

        public async Task<PageResult<AffiliateLinkDTO>> GetAll(int pageSize, int pageNumber)
        {
            var listLink = _context.affiliateLinks.AsNoTracking().AsQueryable();
            var result = listLink.Select(x => _converter.EntityToDTO(x));
            var listPage = Pagination.GetPagedData(result, pageSize, pageNumber);
            return listPage;
        }

        public async Task<ResponseObject<AffiliateLinkDTO>> GetByID(int affiliateLinkID)
        {
            AffiliateLink affiliateLink = await _context.affiliateLinks.SingleOrDefaultAsync(x => x.ID == affiliateLinkID); 
            return _responseObjectAffiliateLinkDTO.ResponseSuccess("Lấy dữ liệu thành công", _converter.EntityToDTO(affiliateLink));
        }
    }
}
