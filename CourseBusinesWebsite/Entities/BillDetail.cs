using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseBusinessWebsite.Entities
{
    public class BillDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int BillID { get; set; }
        public int CourseID { get; set; }
        public double UnitPrice { get; set; }
        public Bill? Bill { get; set; }
        public Course? Course { get; set; }

    }
}
