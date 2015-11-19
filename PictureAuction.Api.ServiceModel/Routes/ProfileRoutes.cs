using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace PictureAuction.Api.ServiceModel.Routes
{
    public static class ProfileRoutes
    {
        [Route("/profile", HttpMethods.Get)]
        public class GetProfile : IReturn<UserDto>
        {
        }

        public class UserDto
        {
            public string Login { get; set; }
            public decimal Bill { get; set; }
        }
    }
}