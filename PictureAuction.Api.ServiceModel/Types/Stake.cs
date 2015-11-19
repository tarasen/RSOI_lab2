#pragma warning disable 1591

using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;
using ServiceStack.OrmLite;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("Stake")]
    public class Stake : IHasId<int>
    {
        [Required]
        [ForeignKey(typeof (Lot))]
        public int LotId { get; set; }

        [Required]
        [ForeignKey(typeof (User))]
        public string Login { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Alias("StakeId")]
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
    }
}

#pragma warning restore 1591