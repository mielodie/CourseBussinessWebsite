using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.ReturnData.AffiliateLinkData;

namespace CourseBusinessWebsite.Payloads.Converters
{
    public class AffiliateConverter
    {
        private readonly CourseConverter _courseConverter;
        private readonly UserConverter _userConverter;
        public AffiliateConverter()
        {
            _courseConverter = new CourseConverter();
            _userConverter = new UserConverter();
        }
        public AffiliateLinkDTO EntityToDTO(AffiliateLink affiliateLink)
        {
            return new AffiliateLinkDTO
            {
                ID = affiliateLink.ID,
                ClickCount = affiliateLink.ClickCount,
                CreateTime = affiliateLink.CreateTime,
                Link = affiliateLink.Link,
                UserID = affiliateLink.UserID,
                CourseID = affiliateLink.CourseID,
                CourseDTO = _courseConverter.EntityToDTO(affiliateLink.Course),
                UserDTO = _userConverter.EntityToDTO(affiliateLink.User)
            };
        }
    }
}

