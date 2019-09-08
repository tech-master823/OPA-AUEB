using NodaTime;
using NodaTime.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AuebUnofficial.Api.Model
{
    public class RSSAnouncement
    {
        public string Kind { get; set; }
        public string Link { get; set; }
        public Instant LastUpdated { get; set; }

        [Obsolete("Property only used for EF-serialization purposes")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DateTime LastUpdatedDateTime
        {
            get => LastUpdated.ToDateTimeUtc();
            set => LastUpdated = DateTime.SpecifyKind(value, DateTimeKind.Utc).ToInstant();
        }
    }
}
