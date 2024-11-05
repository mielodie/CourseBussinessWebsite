using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.ReturnData.CartItemData;

namespace CourseBusinessWebsite.Payloads.ReturnData.CartData
{
    public class CartDTO
    {
        public int UserID { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public IQueryable<CartItemDTO>? CartItemDTOs { get; set; }
    }
}
