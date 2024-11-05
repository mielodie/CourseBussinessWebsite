using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.ReturnData.CartItemData;
using Microsoft.EntityFrameworkCore;

namespace CourseBusinessWebsite.Payloads.Converters
{
    public class CartItemConverter
    {
        private readonly AppDbContext _context;

        public CartItemConverter()
        {
            _context = new AppDbContext();
        }

        public CartItemDTO EntityToDTO(CartItem cartItem)
        {
            return new CartItemDTO
            {
                CourseTitle = _context.courses.SingleOrDefault(x => x.ID ==  cartItem.ID).Title,
                CreateAt = cartItem.CreateAt,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.UnitPrice,
                UpdateAt = cartItem.UpdateAt
            };
        }
    }
}
