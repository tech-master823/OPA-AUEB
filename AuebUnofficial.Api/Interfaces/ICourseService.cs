using AuebUnofficial.Api.Enums;
using AuebUnofficial.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuebUnofficial.Api.Interfaces
{
    public interface ICourseService
    {
        Task<Course> GetCourseAsync(string id);
        Task<OperationsStatusCodes> CreateOrUpdateCourseAync(string id, string token);
    }
}
