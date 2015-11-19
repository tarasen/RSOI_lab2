#pragma warning disable 1591

using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("Cost")]
    public class Cost
    {
        [Required]
        [ForeignKey(typeof (User))]
        public string Login { get; set; }

        [Required]
        [ForeignKey(typeof (Picture))]
        public int PictureId { get; set; }

        [Required]
        public int Value { get; set; }
    }
}

#pragma warning restore 1591