using CourseBusinessWebsite.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseBusinessWebsite.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }
        public DbSet<AffiliateLink> affiliateLinks { get; set; }
        public DbSet<Bill> bills { get; set; }
        public DbSet<BillDetail> billDetails { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<CartItem> cartItems { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Commission> commissions { get; set; }
        public DbSet<CommissionPercent> commissionPercents { get; set; }
        public DbSet<ConfirmEmail> confirmEmails { get; set; }
        public DbSet<ContactAdmin> contactAdmins { get; set; }
        public DbSet<Course> courses { get; set; }
        public DbSet<Lesson> lessons { get; set; }
        public DbSet<LessonStatus> lessonStatuses { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<UserCourse> userCourses { get; set; }
        public DbSet<UserStatus> userStatuses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server = MIMOHON\\SQLEXPRESS; database=CBW; integrated security = sspi; encrypt = true; trustservercertificate = true;");
        }
    }
}
