using System.IO;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace PictureAuction.Api.ServiceModel.Routes
{
    public static class ImageRoutes
    {
        [Route("/img/{Name}.jpg", HttpMethods.Get)]
        public class GetImage : IReturn<Stream>
        {
            public string Name { get; set; }
        }
    }
}