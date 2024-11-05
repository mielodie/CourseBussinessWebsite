using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseBusinessWebsite.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime RemoveAt { get; set; }
        public int RoleID { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public int UserStatusID { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public DateTime LockTime { get; set; }
        public DateTime UnlockTime { get; set; }
        public int NumberOfViolations { get; set; }
        public double TotalMoney { get; set; }
        public Role? Role { get; set; }
        public UserStatus? UserStatus { get; set; }
        public IEnumerable<UserCourse>? UserCourses { get; set; }
        public IEnumerable<RefreshToken>? RefreshTokens { get; set; }
        public IEnumerable<ConfirmEmail>? ConfirmEmails { get; set; }
        public IEnumerable<Cart>? Carts { get; set; }
        public IEnumerable<Bill>? Bills { get; set; }
        public IEnumerable<ContactAdmin>? ContactAdmins { get; set; }

    }
}
