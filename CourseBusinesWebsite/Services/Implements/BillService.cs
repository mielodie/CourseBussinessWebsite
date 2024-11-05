using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.Converters;
using CourseBusinessWebsite.Payloads.RequestData.BillDetailRequest;
using CourseBusinessWebsite.Payloads.RequestData.BillRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.BillData;
using CourseBusinessWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseBusinessWebsite.Services.Implements
{
    public class BillService : IBillService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<BillDTO> _responseObjectBillDTO;
        private readonly BillConverter _converter;

        public BillService(ResponseObject<BillDTO> responseObjectBillDTO, BillConverter converter)
        {
            _context = new AppDbContext();
            _responseObjectBillDTO = responseObjectBillDTO;
            _converter = converter;
        }

        public async Task<ResponseObject<BillDTO>> CreateBill(RequestCreateBill request)
        {
            User user = await _context.users.SingleOrDefaultAsync(x => x.ID == request.UserID);
            if(user == null)
            {
                return _responseObjectBillDTO.ResponseError(StatusCodes.Status404NotFound, "Người dùng không tồn tại", null);
            }
            Bill bill = new Bill();
            bill.CreateAt = DateTime.Now;
            bill.PaymentMethod = request.PaymentMethod;
            bill.StatusName = "CHƯA THANH TOÁN";
            bill.TradingCode = DateTime.Now.Ticks.ToString();
            bill.TotalPrice = 0;
            await _context.bills.AddAsync(bill);
            await _context.SaveChangesAsync();

            bill.BillDetails = await CreateListBillDetail(bill.ID, request.RequestCreateBillDetails);
            await _context.SaveChangesAsync();

            bill.TotalPrice = bill.BillDetails.Sum(x => x.UnitPrice);
            await _context.SaveChangesAsync();
            return _responseObjectBillDTO.ResponseSuccess("Thêm hóa đơn thành công", _converter.EntityToDTO(bill));
        }

        public async Task<List<BillDetail>> CreateListBillDetail(int billID, List<RequestCreateBillDetail> requests)
        {
            Bill bill = await _context.bills.FirstOrDefaultAsync(x => x.ID == billID);
            if( bill is null)
            {
                return null;
            }
            var list = new List<BillDetail>();
            foreach(var request in requests)
            {
                BillDetail billDetail = new BillDetail();
                billDetail.UnitPrice = request.UnitPrice;
                billDetail.CourseID = request.CourseID;
                list.Add(billDetail);
            }
            await _context.AddRangeAsync(list);
            await _context.SaveChangesAsync();
            return list;
        }

        public async Task<ResponseObject<BillDTO>> GetByID(int billID)
        {
            Bill bill = await _context.bills.SingleOrDefaultAsync(x => x.ID == billID);
            return _responseObjectBillDTO.ResponseSuccess("Lấy dữ liệu thành công", _converter.EntityToDTO(bill));
        }
    }
}
