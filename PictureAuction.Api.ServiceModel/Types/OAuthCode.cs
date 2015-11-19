#pragma warning disable 1591

using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("OAuthCode")]
    public class OAuthCode
    {
        [Required]
        [PrimaryKey]
        public Guid Code { get; set; }

        [Required]
        [ForeignKey(typeof (Application))]
        public Guid ClientId { get; set; }

        [Required]
        [ForeignKey(typeof (User))]
        public string UserLogin { get; set; }
    }
}

#pragma warning restore 1591