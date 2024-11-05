using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.Converters;
using CourseBusinessWebsite.Payloads.RequestData.CartRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.CartData;
using CourseBusinessWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseBusinessWebsite.Services.Implements
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<CartDTO> _responseObjectCartDTO;
        private readonly CartConverter _converter;

        public CartService(ResponseObject<CartDTO> responseObjectCartDTO, CartConverter converter)
        {
            _context = new AppDbContext();
            _responseObjectCartDTO = responseObjectCartDTO;
            _converter = converter;
        }

        public async Task<ResponseObject<CartDTO>> CreateCart(RequestCreateCart request)
        {
            User user = await _context.users.FirstOrDefaultAsync(x => x.ID == request.UserID);
            if (user is null)
            {
                return _responseObjectCartDTO.ResponseError(StatusCodes.Status404NotFound, "Người dùng không tồn tại", null);
            }
            Cart cart = new Cart
            {
                CreateAt = DateTime.Now,
                UserID = request.UserID
            };
            await _context.carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return _responseObjectCartDTO.ResponseSuccess("Thêm giỏ hàng thành công", _converter.EntityToDTO(cart));
        }

        public async Task<ResponseObject<CartDTO>> GetByID(int cartID)
        {
            var cart = await _context.carts.SingleOrDefaultAsync(x => x.ID == cartID);
            return _responseObjectCartDTO.ResponseSuccess("Giỏ hàng...", _converter.EntityToDTO(cart));
        }
    }
}
