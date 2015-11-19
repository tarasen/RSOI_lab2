#pragma warning disable 1591

using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;
using ServiceStack.OrmLite;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("Gallery")]
    public class Gallery : IHasId<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [ForeignKey(typeof (Nation))]
        public int NationId { get; set; }

        public string URL { get; set; }

        [Required]
        public DateTime OpenDate { get; set; }

        [Alias("GalleryId")]
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
    }
}

#pragma warning restore 1591