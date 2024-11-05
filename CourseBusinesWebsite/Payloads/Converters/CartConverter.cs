using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.ReturnData.CartData;

namespace CourseBusinessWebsite.Payloads.Converters
{
    public class CartConverter
    {
        private readonly AppDbContext _context;
        private readonly CartItemConverter _converter;

        public CartConverter()
        {
            _context = new AppDbContext();
            _converter = new CartItemConverter();
        }

        public CartDTO EntityToDTO(Cart cart)
        {
            return new CartDTO
            {
                UserID = cart.UserID,
                CreateAt = cart.CreateAt,
                UpdateAt = cart.UpdateAt,
                CartItemDTOs = _context.cartItems.Where(x => x.CartID == cart.ID).Select(x => _converter.EntityToDTO(x))
            };
        }
    }
}
