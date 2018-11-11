using AuebUnofficial.Api.Data;
using AuebUnofficial.Api.Interfaces;
using AuebUnofficial.Api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuebUnofficial.Api.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationsStatusCodes> CreateOrUpdateCourseAync(string id, string token)
        {
            OperationsStatusCodes status;
            if (Exists(id))
            {
                var existingCourse = _context.Courses.Find(id);
                existingCourse.Token = token;
                _context.Courses.Update(existingCourse);
                status = await _context.SaveChangesAsync() > 0
                    ? OperationsStatusCodes.Updated
                    : OperationsStatusCodes.Failed;
            }
            else
            {
                var course = new Course
                {
                    ID = id,
                    Token = token
                };
                _context.Courses.Add(course);
                status = await _context.SaveChangesAsync() > 0
                    ? OperationsStatusCodes.Created
                    : OperationsStatusCodes.Failed;
            }
            return status;
        }

        public async Task<Course> GetCourseAsync(string id)
        {
            return await _context.Courses.FirstOrDefaultAsync(c => c.ID == id);
        }

        private bool Exists(string id)
        {
            return _context.Courses.Any(c => c.ID == id);
        }
    }

    public enum OperationsStatusCodes
    {
        Created, Updated, Failed
    }
}
