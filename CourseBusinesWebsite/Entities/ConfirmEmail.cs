using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseBusinessWebsite.Entities
{
    public class ConfirmEmail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string ConfirmationCode {  get; set; }
        public DateTime ExpirationTime {  get; set; }
        public bool IsConfirmed { get; set; }
        public User? User { get; set; }
    }
}
