#pragma warning disable 1591

using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;
using ServiceStack.OrmLite;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("Lot")]
    public class Lot : IHasId<int>
    {
        [Required]
        [ForeignKey(typeof (Picture))]
        public int PictureId { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Alias("LotId")]
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
    }
}

#pragma warning restore 1591