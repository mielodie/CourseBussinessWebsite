using CourseBusinessWebsite.Payloads.RequestData.BillDetailRequest;
using CourseBusinessWebsite.Payloads.ReturnData.BillDetailData;

namespace CourseBusinessWebsite.Payloads.RequestData.BillRequest
{
    public class RequestCreateBill
    {
        public int UserID { get; set; }
        public string PaymentMethod { get; set; }
        public List<RequestCreateBillDetail> RequestCreateBillDetails { get; set; }
    }
}
