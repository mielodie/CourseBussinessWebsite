using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseBusinessWebsite.Entities
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string AvatarCourse { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime RemoveAt { get; set; }
        public int Duration { get; set; }
        public int NumberOfPurchases { get; set; }
        public string Creator { get; set; }
        public int CategoryID { get; set; }
        public bool isActive { get; set; }
        public Category? Category { get; set; }
        public IEnumerable<UserCourse>? UserCourses { get; set; }
        public IEnumerable<Lesson>? Lessons { get; set; }
        public IEnumerable<BillDetail>? BillDetails { get; set; }
        public IEnumerable<CartItem>? CartItems { get; set; }
    }
}
