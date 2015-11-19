#pragma warning disable 1591

using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("Material")]
    public class Material : IHasId<int>
    {
        [Required]
        public string Name { get; set; }

        [Alias("MaterialId")]
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
    }
}

#pragma warning restore 1591