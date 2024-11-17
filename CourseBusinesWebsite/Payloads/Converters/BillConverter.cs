using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.ReturnData.BillData;

namespace CourseBusinessWebsite.Payloads.Converters
{
    public class BillConverter
    {
        private readonly AppDbContext _context;
        private readonly BillDetailConverter _converter;

        public BillConverter()
        {
            _context = new AppDbContext();
            _converter = new BillDetailConverter();
        }

        public BillDTO EntityToDTO(Bill bill)
        {
            return new BillDTO
            {
                ID = bill.ID,
                CreateAt = bill.CreateAt,
                PaymentMethod = bill.PaymentMethod,
                PaymentTime = bill.PaymentTime,
                StatusName = bill.StatusName,
                TotalPrice = bill.TotalPrice,
                TradingCode = bill.TradingCode,
                UpdateAt = bill.UpdateAt,
                UserID = bill.UserID,
                BillDetailDTOs = _context.billDetails.Where(x => x.BillID == bill.ID).Select(x => _converter.EntityToDTO(x))
            };
        }
    }
}
