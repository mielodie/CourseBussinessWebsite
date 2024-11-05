using CourseBusinessWebsite.Payloads.ReturnData.BillDetailData;

namespace CourseBusinessWebsite.Payloads.ReturnData.BillData
{
    public class BillDTO
    {
        public int UserID { get; set; }
        public string StatusName { get; set; }
        public string TradingCode { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? PaymentTime { get; set; }
        public double? TotalPrice { get; set; }
        public IQueryable<BillDetailDTO> BillDetailDTOs { get; set; }
    }
}
