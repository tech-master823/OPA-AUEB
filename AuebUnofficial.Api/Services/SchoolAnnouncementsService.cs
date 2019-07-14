using AuebUnofficial.Api.Data;
using AuebUnofficial.Api.Enums;
using AuebUnofficial.Api.Model;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System;
using System.Collections.Generic;
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
                var existingRssAnnouncemet = _context.RSSAnouncements.Where(a=> a.Kind.Equals(kind)).SingleOrDefault();
                existingRssAnnouncemet.Link = link;
                _context.RSSAnouncements.Update(existingRssAnnouncemet);
                status = await _context.SaveChangesAsync() > 0
                    ? OperationsStatusCodes.Updated
                    : OperationsStatusCodes.Failed;
            }
            else
            {
                var rssAnnouncements = new RSSAnouncements
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

        public async Task<RSSAnouncements> GetAnnouncementsAsync(string kind)
        {
            return await _context.RSSAnouncements.FirstOrDefaultAsync(a => a.Kind.Equals(kind));
        }
    }
}
