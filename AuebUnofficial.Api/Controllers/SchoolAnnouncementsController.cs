using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuebUnofficial.Api.Enums;
using AuebUnofficial.Api.Interfaces;
using AuebUnofficial.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AuebUnofficial.Api.Controllers
{
    [Route("api/announcements")]
    [ApiController]
    public class SchoolAnnouncementsController : ControllerBase
    {
        private readonly ISchoolAnnouncementsService _schoolAnnouncementsService;
        private readonly IConfiguration Configuration;

        public SchoolAnnouncementsController(ISchoolAnnouncementsService schoolAnnouncementsService, IConfiguration configuration)
        {
            _schoolAnnouncementsService = schoolAnnouncementsService;
            Configuration = configuration;
        }

        [HttpGet("{kind}", Name = "GetAnnouncements")]
        public async Task<ActionResult<RSSAnouncement>> GetByKind(string kind)
        {
            var schoolAnnouncements = await _schoolAnnouncementsService.GetAnnouncementsAsync(kind);
            if (schoolAnnouncements == null)
                return NotFound();
            var lastUpdated = schoolAnnouncements.LastUpdated.ToDateTimeUtc();
            return Ok(new { schoolAnnouncements.Kind, schoolAnnouncements.Link, lastUpdated});
        }

        [HttpPost("{apikey}", Name = "CreateAnnouncent")]
        public async Task<IActionResult> CreateOrUpdateAnnouncement(string apikey, RSSAnouncement schoolAnnouncements)
        {
            if (string.IsNullOrEmpty(Configuration["adminApiKey"])) return BadRequest(System.Net.HttpStatusCode.InternalServerError);

            if (!apikey.Equals(Configuration["adminApiKey"]))
                return Forbid();

            if (!ModelState.IsValid)
                return BadRequest();
            var status = await _schoolAnnouncementsService.CreateOrUpdateSchoolAnnouncementAsync(schoolAnnouncements.Kind, schoolAnnouncements.Link);

            if (status == OperationsStatusCodes.Failed)
                return BadRequest("Failed to create or update module");

            if (status == OperationsStatusCodes.Updated)
                return NoContent();

            return CreatedAtRoute("GetAnnouncements", new { kind = schoolAnnouncements.Kind}, schoolAnnouncements);
        }
    }
}