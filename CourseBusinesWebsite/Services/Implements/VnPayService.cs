using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Handle.EmailHandle;
using CourseBusinessWebsite.Handle.VnPayHandle;
using CourseBusinessWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseBusinessWebsite.Services.Implements
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly VnPayLib pay;
        private readonly AuthService _authService;
        public readonly AppDbContext _context;
        public VnPayService(IConfiguration configuration, AuthService authService)
        {
            _configuration = configuration;
            pay = new VnPayLib();
            _authService = authService;
            _context = new AppDbContext();
        }
        public async Task<string> CreatePaymentUrl(int billID, HttpContext httpContext, int id)
        {
            var bill = await _context.bills.SingleOrDefaultAsync(x => x.ID == billID);
            var user = await _context.users.SingleOrDefaultAsync(x => x.ID == id);
            if (user.ID == bill.UserID)
            {
                if (bill.BillStatus == Enum.BillStatus.PAID)
                {
                    return "Hóa đơn đã được thanh toán trước đó";
                }
                if (bill.BillStatus == Enum.BillStatus.PENDING && bill.TotalPrice != 0 && bill.TotalPrice != null)
                {
                    pay.AddRequestData("vnp_Version", "2.1.0");
                    pay.AddRequestData("vnp_Command", "pay");
                    pay.AddRequestData("vnp_TmnCode", "YIK14C5R");
                    pay.AddRequestData("vnp_Amount", (bill.TotalPrice * 100).ToString());
                    pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    pay.AddRequestData("vnp_CurrCode", "VND");
                    pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(httpContext));
                    pay.AddRequestData("vnp_Locale", "vn");
                    pay.AddRequestData("vnp_OrderInfo", $"Thanh taons hóa đơn{billID}");
                    pay.AddRequestData("vnp_OrderType", "other");
                    pay.AddRequestData("vnp_ReturnUrl", _configuration.GetSection("VnPay:vnp_ReturnUrl").Value);
                    pay.AddRequestData("vnp_TxnRef", billID.ToString());

                    string paymentUrl = pay.CreateRequestUrl(_configuration.GetSection("VnPay:vnp_Url").Value, _configuration.GetSection("VnPay:vnp_HashSecret").Value);
                    return paymentUrl;
                }
                else
                {
                    return "Vui lòng kiểm tra lại hóa đơn";
                }
            }
            return "Vui lòng kiểm tra lại hóa đơn";
        }

        public async Task<string> VNPayReturn(IQueryCollection vnpayData)
        {
            string vnp_TmnCode = _configuration.GetSection("VnPay:vnp_TmnCode").Value;
            string vnp_HashSecret = _configuration.GetSection("VnPay:vnp_HashSecret").Value;

            VnPayLib vnPayLibrary = new VnPayLib();
            foreach (var (key, value) in vnpayData)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnPayLibrary.AddResponseData(key, value);
                }
            }

            string billId = vnPayLibrary.GetResponseData("vnp_TxnRef");
            string vnp_ResponseCode = vnPayLibrary.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnPayLibrary.GetResponseData("vnp_TransactionStatus");
            string vnp_SecureHash = vnPayLibrary.GetResponseData("vnp_SecureHash");
            double vnp_Amount = Convert.ToDouble(vnPayLibrary.GetResponseData("vnp_Amount"));
            bool check = vnPayLibrary.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

            if (check)
            {/**/
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    var bill = await _context.bills.FirstOrDefaultAsync(x => x.ID == Convert.ToInt32(billId));

                    if (bill == null)
                    {
                        return "Không tìm thấy hóa đơn";
                    }

                    bill.BillStatus = Enum.BillStatus.PENDING;
                    bill.CreateAt = DateTime.Now;

                    _context.bills.Update(bill);
                    await _context.SaveChangesAsync();

                    var user = _context.users.FirstOrDefault(x => x.ID == bill.UserID);
                    if (user != null)
                    {
                        string email = user.Email;
                        string mss = _authService.SendEmail(new EmailTo
                        {
                            Mail = email,
                            Subject = $"Thanh Toán đơn hàng: {bill.ID}",
                            Content = BillEmailTemplate.GenerateNotificationBillEmail(bill, "THANH TOÁN")
                        });
                    }

                    return "Giao dịch thành công đơn hàng " + bill.ID;
                }
                else
                {
                    return $"Lỗi trong khi thực hiện giao dịch. Mã lỗi: {vnp_ResponseCode}";
                }
            }
            else
            {
                return "Có lỗi trong quá trình xử lý";
            }
        }
    }
}
