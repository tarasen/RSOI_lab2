#pragma warning disable 1591

using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("PicturesByArtist")]
    public class PicturesByArtist
    {
        [Required]
        [ForeignKey(typeof (PicturesByArtist))]
        public int PictureId { get; set; }

        [Required]
        [ForeignKey(typeof (Artist))]
        public int ArtistId { get; set; }
    }
}

#pragma warning restore 1591