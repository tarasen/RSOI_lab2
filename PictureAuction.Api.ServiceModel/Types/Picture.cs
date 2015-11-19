#pragma warning disable 1591

using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;
using ServiceStack.OrmLite;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("Picture")]
    public class Picture : IHasId<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Creation { get; set; }

        [Required]
        [ForeignKey(typeof (Material))]
        public int MaterialId { get; set; }

        [Required]
        [ForeignKey(typeof (Technique))]
        public int TechniqueId { get; set; }

        [ForeignKey(typeof (Gallery))]
        public int? GalleryId { get; set; }

        public decimal? StartCost { get; set; }
        public double? Height { get; set; }
        public double? Width { get; set; }

        [Required]
        public bool IsSaleable { get; set; }

        [Alias("PictureId")]
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
    }
}

#pragma warning restore 1591