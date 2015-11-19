#pragma warning disable 1591

using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("Application")]
    public class Application
    {
        [Required]
        [PrimaryKey]
        public Guid ClientId { get; set; }

        [Required]
        public Guid SecretKey { get; set; }

        [Required]
        public string RedirectUri { get; set; }

        [Required]
        [ForeignKey(typeof (User))]
        public string Owner { get; set; }
    }
}

#pragma warning restore 1591