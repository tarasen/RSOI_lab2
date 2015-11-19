#pragma warning disable 1591

using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("Technique")]
    public class Technique : IHasId<int>
    {
        [Required]
        public string Name { get; set; }

        [Alias("TechniqueId")]
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
    }
}

#pragma warning restore 1591