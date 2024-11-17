using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseBusinessWebsite.Entities
{
    public class CommissionPercent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CourseID { get; set; }
        public double Percent { get; set; }
        public Course? Course { get; set; }
    }
}
