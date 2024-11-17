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
                ID = billDetail.ID,
                CourseID = billDetail.CourseID,
                UnitPrice = billDetail.UnitPrice
            };
        }
    }
}
