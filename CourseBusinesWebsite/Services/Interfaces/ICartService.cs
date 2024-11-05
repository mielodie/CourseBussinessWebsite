using CourseBusinessWebsite.Payloads.RequestData.CartRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.CartData;

namespace CourseBusinessWebsite.Services.Interfaces
{
    public interface ICartService
    {
        Task<ResponseObject<CartDTO>> GetByID(int cartID);
        Task<ResponseObject<CartDTO>> CreateCart(RequestCreateCart request);
    }
}
