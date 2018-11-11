using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AuebUnofficial.Api.Model
{
    public class Day2Day
    {
        [Key]
        public Semester Semester { get; set; } //semester: fall or spring

        public string Link { get; set; }
    }
    public enum Semester
    {
        Fall, Spring
    }
}