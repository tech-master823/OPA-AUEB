using System;
using System.ComponentModel.DataAnnotations;

namespace AuebUnofficial.Api.Model
{
    public class Course
    {
        [Required]
        public String ID { get; set; }
        [Required]
        public String Token { get; set; }

    }
}