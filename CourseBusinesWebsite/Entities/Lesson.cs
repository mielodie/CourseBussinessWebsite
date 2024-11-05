using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseBusinessWebsite.Entities
{
    public class Lesson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CourseID { get; set; }
        public string LessonName { get; set; }
        public string ContentURL { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public int Duration { get; set; }
        public int Order { get; set; }
        public int LessonStatusID { get; set; }
        public LessonStatus? LessonStatus { get; set; }
        public Course? Course { get; set; }
    }
}
