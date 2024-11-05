namespace CourseBusinessWebsite.Payloads.ReturnData.CartItemData
{
    public class CartItemDTO
    {
        public string CourseTitle { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
