using CourseBusinessWebsite.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseBusinessWebsite.Entities
{
    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string StatusName { get; set; }
        public string TradingCode { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? PaymentTime { get; set; }
        public double? TotalPrice { get; set; }
        public BillStatus BillStatus { get; set; } 
        public User? User { get; set; }
        public IEnumerable<BillDetail>? BillDetails { get; set; }
    }
}
