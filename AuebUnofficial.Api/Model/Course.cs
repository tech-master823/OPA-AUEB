using System;
using System.ComponentModel.DataAnnotations;

namespace AuebUnofficial.Api.Model
{
    public class Course
    {
        [Key]
        public String ID { get; set; }
        public String Token { get; set; }

    }
}