using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace PictureAuction.Api.ServiceModel.Routes
{
    public static class AuthRoutes
    {
        [Route("/auth", HttpMethods.Get)]
        [DataContract]
        public class AuthRequest : IReturn<AuthResponse>
        {
            [DataMember(Name = "client_id")]
            public Guid ClientId { get; set; }

            [DataMember(Name = "state")]
            public string State { get; set; }

            [DataMember(Name = "redirect_uri")]
            public string RedirectUri { get; set; }
        }

        public class AuthResponse
        {
            public Guid ClientId { get; set; }
            public string State { get; set; }
            public string RedirectUri { get; set; }
        }

        [Route("/login", HttpMethods.Post)]
        public class LoginRequest : AuthResponse, IReturnVoid
        {
            public string Login { get; set; }
            public string Password { get; set; }

            public bool IsNotValid()
            {
                var strings = new[] {Login, Password, RedirectUri};
                return strings.Any(string.IsNullOrWhiteSpace) && ClientId == Guid.Empty;
            }
        }

        [Route("/token", HttpMethods.Post)]
        public class CreateToken : IReturn<TokenResponse>
        {
            public Guid Code { get; set; }
            public string RedirectUri { get; set; }
            public Guid ClientId { get; set; }
            public Guid SecretKey { get; set; }

            public bool IsNotValid()
            {
                return ClientId == Guid.Empty || SecretKey == Guid.Empty
                       || Code == Guid.Empty || string.IsNullOrEmpty(RedirectUri);
            }
        }

        public class TokenResponse
        {
            public Guid AccessToken { get; set; }
            public long ExpiresIn { get; set; }
            public string TokenType { get; set; } = "Bearer";
        }
    }
}