﻿using System.ComponentModel.DataAnnotations;

namespace CourseBusinessWebsite.Payloads.RequestData.CourseRequest
{
    public class RequestUpdateCourse
    {
        public int CourseID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile AvatarCourse { get; set; }
        public int Duration { get; set; }
        public string Creator { get; set; }
        public int CategoryID { get; set; }
    }
}
