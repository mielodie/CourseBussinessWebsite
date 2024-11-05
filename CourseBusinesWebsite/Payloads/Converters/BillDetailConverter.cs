using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.ReturnData.BillDetailData;

namespace CourseBusinessWebsite.Payloads.Converters
{
    public class BillDetailConverter
    {
        public BillDetailDTO EntityToDTO(BillDetail billDetail)
        {
            return new BillDetailDTO
            {
                CourseID = billDetail.CourseID,
                UnitPrice = billDetail.UnitPrice
            };
        }
    }
}
