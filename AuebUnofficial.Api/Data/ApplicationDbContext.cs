using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuebUnofficial.Core.Model;

namespace AuebUnofficial.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Model.RSSAnouncement> RSSAnouncements { get; set; }
    }
}
