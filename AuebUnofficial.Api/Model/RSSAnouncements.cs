using NodaTime;
using NodaTime.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuebUnofficial.Api.Model
{
    public class RSSAnouncements
    {
        [Required, Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Kind { get; set; }
        [Required]
        public string Link { get; set; }
        [NotMapped]
        public Instant LastUpdated { get; set; }

        [Obsolete("Property only used for EF-serialization purposes")]
        [DataType(DataType.DateTime)]
        [Column("LastUpdatedDateTime")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DateTime LastUpdatedDateTime
        {
            get => LastUpdated.ToDateTimeUtc();
            set => LastUpdated = DateTime.SpecifyKind(value, DateTimeKind.Utc).ToInstant();
        }
    }
}
