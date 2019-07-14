using AuebUnofficial.Api.Enums;
using AuebUnofficial.Api.Interfaces;
using AuebUnofficial.Api.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuebUnofficial.Api.Controllers
{
    [Route("api/course")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("{id}", Name = "GetCourse")]
        public async Task<ActionResult<Course>> GetById(string id)
        {
            var course = await _courseService.GetCourseAsync(id);
            if (course == null)
                return NotFound();

            return Ok(course);
        }

        [HttpPost(Name ="CreateCourse")]
        public async Task<IActionResult> CreateOrUpdateCourse(Course course)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var status = await _courseService.CreateOrUpdateCourseAync(course.ID, course.Token);

            if (status == OperationsStatusCodes.Failed)
                return BadRequest();

            if (status == OperationsStatusCodes.Updated)
                return NoContent();
            
            return CreatedAtRoute("GetCourse", new { id = course.ID }, course);
        }
    }
}