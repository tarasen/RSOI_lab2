#pragma warning disable 1591

using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace PictureAuction.Api.ServiceModel.Types
{
    [Alias("Token")]
    public class Token
    {
        [Required]
        [PrimaryKey]
        public Guid AccessToken { get; set; }

        [Required]
        public DateTime ExpiresIn { get; set; }

        [Required]
        [ForeignKey(typeof (OAuthCode))]
        public Guid Code { get; set; }
    }
}

#pragma warning restore 1591