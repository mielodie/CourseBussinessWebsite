using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseBusinessWebsite.Entities
{
    public class UserCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public int PercentCompleted { get; set; }
        public DateTime CourseRegistrationPeriod { get; set; }
        public bool IsCompleted { get; set; }
        public User? User { get; set; }
        public Course? Course { get; set; }
    }
}
