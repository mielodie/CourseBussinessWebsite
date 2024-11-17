using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseBusinessWebsite.Entities
{
    public class Commission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int AffiliateLinkID { get; set; }
        public int CollaboratorID { get; set; }
        public double CommissionAmount { get; set; }
        public User? User { get; set; }
        public AffiliateLink? AffiliateLink { get; set;}
        
    }
}
