using System;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using PictureAuction.Api.ServiceModel.Types;
using ServiceStack.Common.Extensions;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using static System.Net.HttpStatusCode;
using static PictureAuction.Api.ServiceModel.Routes.AuthRoutes;

namespace PictureAuction.Api.ServiceInterface.Services
{
    public class AuthService : Service
    {
        static AuthService()
        {
            Mapper.CreateMap<Token, TokenResponse>()
                .ForMember(x => x.ExpiresIn,
                    expression => expression.MapFrom(e => (e.ExpiresIn - DateTime.Now).Milliseconds));
        }

        public static Token CheckPermitions(string authHeader, IDbConnection db)
        {
            if (authHeader.IsNullOrEmpty())
                return null;

            if (!authHeader.StartsWith("Bearer ", StringComparison.Ordinal))
                return null;

            Guid token;
            if (!Guid.TryParse(authHeader.Substring(7), out token))
                return null;

            var dbToken = db.FirstOrDefaultById<Token>(token);
            if (dbToken == null || dbToken.ExpiresIn < DateTime.Now)
                return null;

            dbToken.ExpiresIn = DateTime.Now + TimeSpan.FromDays(7);
            db.Update(dbToken);
            return dbToken;
        }

        [DefaultView("Login")]
        public object Get(AuthRequest request)
        {
            try
            {
                return
                    new HttpResult(new AuthResponse
                    {
                        RedirectUri = request.RedirectUri,
                        ClientId = request.ClientId,
                        State = request.State
                    });
            }
            catch
            {
                return new HttpError(InternalServerError, "Internal Server Error");
            }
        }

        public object Post(LoginRequest request)
        {
            try
            {
                if (request.IsNotValid())
                    return new HttpResult {StatusCode = BadRequest};

                var app = Db.FirstOrDefaultById<Application>(request.ClientId);
                if (app == null || app.RedirectUri != request.RedirectUri)
                    return new HttpResult {StatusCode = BadRequest};

                var user = Db.FirstOrDefaultById<User>(request.Login);
                if (user == null)
                    return new HttpResult {StatusCode = BadRequest};

                var enc = Encoding.UTF8;
                string hashString;

                using (var sha1 = SHA1.Create())
                {
                    var hash = sha1.ComputeHash(enc.GetBytes(request.Password + user.Salt));
                    hashString = string.Join("", hash.Select(x => x.ToString("x")));
                }

                if (user.PasswordHash != hashString)
                    return new HttpResult {StatusCode = BadRequest};

                var code = new OAuthCode {ClientId = app.ClientId, UserLogin = user.Login, Code = Guid.NewGuid()};
                Db.Save(code);

                return new HttpResult
                {
                    StatusCode = Redirect,
                    Headers =
                    {
                        {
                            HttpHeaders.Location,
                            new UriBuilder(app.RedirectUri)
                            {
                                Query = $"code={code.Code}{(request.State == null ? "" : $"&state={request.State}")}"
                            }
                                .Uri.AbsoluteUri
                        }
                    }
                };
            }
            catch
            {
                return new HttpError(InternalServerError, "Internal Server Error");
            }
        }

        public object Post(CreateToken request)
        {
            try
            {
                if (request.IsNotValid())
                    return new HttpResult {StatusCode = BadRequest};

                var oauth = Db.FirstOrDefaultById<OAuthCode>(request.Code);
                if (oauth == null || oauth.ClientId != request.ClientId)
                    return new HttpResult {StatusCode = BadRequest};

                var app = Db.FirstOrDefaultById<Application>(request.ClientId);
                if (app == null || app.SecretKey != request.SecretKey || app.RedirectUri != request.RedirectUri)
                    return new HttpResult {StatusCode = BadRequest};

                var token = new Token
                {
                    AccessToken = Guid.NewGuid(),
                    ExpiresIn = DateTime.Now + TimeSpan.FromDays(7),
                    Code = oauth.Code
                };

                Db.Save(token);
                return new HttpResult(Mapper.Map<TokenResponse>(token), $"{MimeTypes.Json}; charset=utf-8")
                {
                    StatusCode = Created
                };
            }
            catch
            {
                return new HttpError(InternalServerError, "Internal Server Error");
            }
        }
    }
}