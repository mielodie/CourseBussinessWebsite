﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseBusinessWebsite.Entities
{
    public class AffiliateLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public string Link { get; set; }
        public int ClickCount { get; set; }
        public DateTime CreateTime { get; set; }
        public User? User { get; set; }
        public Course? Course { get; set; }
        public IEnumerable<Commission>? Commissions { get; set; }
    }
}
