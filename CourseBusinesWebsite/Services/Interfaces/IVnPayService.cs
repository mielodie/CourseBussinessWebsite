namespace CourseBusinessWebsite.Services.Interfaces
{
    public interface IVnPayService
    {
        Task<string> CreatePaymentUrl(int billID, HttpContext httpContext, int id);
        Task<string> VNPayReturn(IQueryCollection vnpayData);
    }
}
