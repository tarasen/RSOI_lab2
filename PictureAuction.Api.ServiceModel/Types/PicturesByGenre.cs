#pragma warning disable 1591

using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("PicturesByGenre")]
    public class PicturesByGenre
    {
        [Required]
        [ForeignKey(typeof (Picture))]
        public int PictureId { get; set; }

        [Required]
        [ForeignKey(typeof (Genre))]
        public int GenreId { get; set; }
    }
}

#pragma warning restore 1591