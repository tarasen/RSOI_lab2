#pragma warning disable 1591

using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;
using ServiceStack.OrmLite;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("Genre")]
    public class Genre : IHasId<int>
    {
        [Required]
        public string Name { get; set; }

        [ForeignKey(typeof (Nation))]
        public int? NationId { get; set; }

        public string Date { get; set; }

        [Alias("GenreId")]
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
    }
}

#pragma warning restore 1591