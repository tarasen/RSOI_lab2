#pragma warning disable 1591

using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("TUser")]
    public class User
    {
        [Required]
        [PrimaryKey]
        public string Login { get; set; }

        [ForeignKey(typeof (Artist))]
        public int? ArtistId { get; set; }

        [Required]
        public decimal Bill { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [Alias("Solt")]
        public string Salt { get; set; }
    }
}

#pragma warning restore 1591