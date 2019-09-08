using AuebUnofficial.Api.Data;
using AuebUnofficial.Api.Enums;
using AuebUnofficial.Api.Model;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AuebUnofficial.Api.Services
{
    public class SchoolAnnouncementsService : Interfaces.ISchoolAnnouncementsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IClock _clock;
        public SchoolAnnouncementsService(ApplicationDbContext context, IClock clock)
        {
            _context = context;
            _clock = clock;
        }

        public async Task<OperationsStatusCodes> CreateOrUpdateSchoolAnnouncementAsync(string kind, string link)
        {
            OperationsStatusCodes status;
            if (Exists(kind))
            {
                var existingRssAnnouncement = _context.RSSAnouncements.SingleOrDefault(a => a.Kind.Equals(kind));
                existingRssAnnouncement.Link = link;
                _context.RSSAnouncements.Update(existingRssAnnouncement);
                status = await _context.SaveChangesAsync() > 0
                    ? OperationsStatusCodes.Updated
                    : OperationsStatusCodes.Failed;
            }
            else
            {
                var rssAnnouncements = new RSSAnouncement
                {
                    Kind = kind,
                    Link = link,
                    LastUpdated = _clock.GetCurrentInstant()
                };
                await _context.RSSAnouncements.AddAsync(rssAnnouncements);
                status = await _context.SaveChangesAsync() > 0
                    ? OperationsStatusCodes.Created
                    : OperationsStatusCodes.Failed;
            }
            return status;
        }

        private bool Exists(string kind)
        {
            return _context.RSSAnouncements.Any(a => a.Kind.Equals(kind));
        }

        public async Task<RSSAnouncement> GetAnnouncementsAsync(string kind)
        {
            return await _context.RSSAnouncements.FirstOrDefaultAsync(a => a.Kind.Equals(kind));
        }
    }
}
