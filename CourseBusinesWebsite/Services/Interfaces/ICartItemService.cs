using CourseBusinessWebsite.Payloads.RequestData.CartItemRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.CartItemData;

namespace CourseBusinessWebsite.Services.Interfaces
{
    public interface ICartItemService
    {
        Task<ResponseObject<CartItemDTO>> CreateCartItem(RequestCreateCartItem request);
        Task<ResponseObject<CartItemDTO>> UpdateCartItem(RequestUpdateCartItem request);
        Task<string> RemoveCartItem(int cartItemID);
        Task<List<CartItemDTO>> GetAll();
        Task<ResponseObject<CartItemDTO>> GetByID(int cartItemID);
    }
}
