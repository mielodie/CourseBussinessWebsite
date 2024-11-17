using CloudinaryDotNet;
using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.Converters;
using CourseBusinessWebsite.Payloads.RequestData.CartItemRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.CartItemData;
using CourseBusinessWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseBusinessWebsite.Services.Implements
{
    public class CartItemService : ICartItemService
    {
        private readonly ResponseObject<CartItemDTO> _responseObjectCartItemDTO;
        private readonly AppDbContext _context;
        private readonly CartItemConverter _converter;

        public CartItemService(ResponseObject<CartItemDTO> responseObjectCartItemDTO, CartItemConverter converter)
        {
            _context = new AppDbContext();
            _responseObjectCartItemDTO = responseObjectCartItemDTO;
            _converter = converter;
        }

        public async Task<ResponseObject<CartItemDTO>> CreateCartItem(RequestCreateCartItem request)
        {
            Cart cart = await _context.carts.FirstOrDefaultAsync(x => x.ID == request.CartID);
            if(cart is null)
            {
                return _responseObjectCartItemDTO.ResponseError(StatusCodes.Status404NotFound, "Giỏ hàng không tồn tại", null);
            }
            Course course = await _context.courses.FirstOrDefaultAsync(x => x.ID == request.CourseID);
            if (course is null)
            {
                return _responseObjectCartItemDTO.ResponseError(StatusCodes.Status404NotFound, "Khóa học không tồn tại", null);
            }
            CartItem cartItem = new CartItem();
            cartItem.CreateAt = DateTime.Now;
            cartItem.Quantity = 1;
            cartItem.UnitPrice = course.Price;
            await _context.cartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            return _responseObjectCartItemDTO.ResponseSuccess("Thêm khóa học vào giỏ hàng thành công", _converter.EntityToDTO(cartItem));
        }

        public async Task<List<CartItemDTO>> GetAll()
        {
            var list = await _context.cartItems.AsNoTracking().ToListAsync();
            return list.Select(x => _converter.EntityToDTO(x)).ToList();
        }

        public async Task<ResponseObject<CartItemDTO>> GetByID(int cartItemID)
        {
            CartItem cartItem = await _context.cartItems.FirstOrDefaultAsync(x => x.ID == cartItemID);
            return _responseObjectCartItemDTO.ResponseSuccess("Lấy dữ liệu thành công", _converter.EntityToDTO(cartItem));
        }

        public async Task<string> RemoveCartItem(int cartItemID)
        {
            CartItem cartItem = await _context.cartItems.FirstOrDefaultAsync(x => x.ID == cartItemID);
            if (cartItem is null)
            {
                return "Khóa học trong giỏ không tồn tại";
            }
            _context.cartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return "Xóa khóa học khỏi giỏ hàng thành công";
        }

        public async Task<ResponseObject<CartItemDTO>> UpdateCartItem(RequestUpdateCartItem request)
        {
            CartItem cartItem = await _context.cartItems.FirstOrDefaultAsync(x => x.ID == request.CartItemID);
            if (cartItem is null)
            {
                return _responseObjectCartItemDTO.ResponseError(StatusCodes.Status404NotFound, "Khóa học trong giỏ không tồn tại", null);
            }
            cartItem.Quantity = 1;
            cartItem.CourseID = request.CourseID;
            cartItem.UnitPrice = _context.courses.FirstOrDefault(x => x.ID == request.CourseID).Price;
            cartItem.UpdateAt = DateTime.Now;
            _context.cartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            return _responseObjectCartItemDTO.ResponseSuccess("Sửa giỏ hàng thành công", _converter.EntityToDTO(cartItem));
        }
    }
}
