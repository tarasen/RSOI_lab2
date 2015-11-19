using System.Net;
using AutoMapper;
using PictureAuction.Api.ServiceModel.Types;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using static PictureAuction.Api.ServiceModel.Routes.ProfileRoutes;
using static ServiceStack.Common.Web.HttpError;

namespace PictureAuction.Api.ServiceInterface.Services
{
    public class ProfileService : Service
    {
        static ProfileService()
        {
            Mapper.CreateMap<User, UserDto>();
        }

        public object Get(GetProfile request)
        {
            try
            {
                var token = AuthService.CheckPermitions(Request.Headers["Authorization"], Db);
                if (token == null)
                    return Unauthorized("Please, authorize");


                var code = Db.GetByIdOrDefault<OAuthCode>(token.Code);
                if (code == null)
                    return NotFound("");

                var user = Db.GetById<User>(code.UserLogin);
                if (user == null)
                    return NotFound("");

                return new HttpResult(Mapper.Map<UserDto>(user), $"{MimeTypes.Json}; charset=utf-8");
            }
            catch
            {
                return new HttpError(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
    }
}