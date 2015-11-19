#pragma warning disable 1591

using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;
using ServiceStack.OrmLite;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("Artist")]
    public class Artist : IHasId<int>
    {
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        [Required]
        public string BirthDate { get; set; }

        public string DeathDate { get; set; }

        [Required]
        [ForeignKey(typeof (Nation))]
        public int NationId { get; set; }

        [Alias("ArtistId")]
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
    }
}

#pragma warning restore 1591