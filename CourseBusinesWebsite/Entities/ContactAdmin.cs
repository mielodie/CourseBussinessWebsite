using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseBusinessWebsite.Entities
{
    public class ContactAdmin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ContactPersonID { get; set; }
        public string ContactPersonName { get; set; }
        public string PhoneNumber { get; set;}
        public DateTime ContactAt { get; set; }
        public bool IsContacted { get; set; }
        public User? User { get; set; }
    }
}
