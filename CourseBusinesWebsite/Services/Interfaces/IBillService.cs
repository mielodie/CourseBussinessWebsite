using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.RequestData.BillDetailRequest;
using CourseBusinessWebsite.Payloads.RequestData.BillRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.BillData;

namespace CourseBusinessWebsite.Services.Interfaces
{
    public interface IBillService
    {
        Task<List<BillDetail>> CreateListBillDetail(int billID, List<RequestCreateBillDetail> requests);
        Task<ResponseObject<BillDTO>> CreateBill(RequestCreateBill request);
        Task<ResponseObject<BillDTO>> GetByID(int billID);
    }
}
