#pragma warning disable 1591

using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("ArtistsByPeriod")]
    public class ArtistsByPeriod
    {
        [Required]
        [ForeignKey(typeof (Artist))]
        public int ArtistId { get; set; }

        [Required]
        [ForeignKey(typeof (Period))]
        public int PeriodId { get; set; }
    }
}

#pragma warning restore 1591