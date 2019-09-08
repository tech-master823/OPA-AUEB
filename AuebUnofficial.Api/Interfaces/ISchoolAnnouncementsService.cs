using AuebUnofficial.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using AuebUnofficial.Api.Enums;
using System.Threading.Tasks;

namespace AuebUnofficial.Api.Interfaces
{
    public interface ISchoolAnnouncementsService
    {
        Task<RSSAnouncement> GetAnnouncementsAsync(string kind);
        Task<OperationsStatusCodes> CreateOrUpdateSchoolAnnouncementAsync(string kind, string link);
    }
}
