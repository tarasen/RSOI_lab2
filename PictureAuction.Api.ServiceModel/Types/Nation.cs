#pragma warning disable 1591

using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("Nation")]
    public class Nation : IHasId<int>
    {
        public string Flag { get; set; }

        [Required]
        public string Name { get; set; }

        [Alias("NationId")]
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
    }
}

#pragma warning restore 1591