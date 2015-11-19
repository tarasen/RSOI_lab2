using System;
using System.Configuration;
using System.IO;
using System.Net;
using Funq;
using PictureAuction.Api.ServiceInterface.Services;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using ServiceStack.Razor;
using ServiceStack.ServiceHost;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.WebHost.Endpoints.Formats;

namespace PictureAuction.Api.Host
{
    public class AppHost : AppHostHttpListenerBase
    {
        public AppHost()
            : base("PictureAuction.Api.Host", typeof (ImageService).Assembly)
        {
        }

        public override void Configure(Container container)
        {
            container.Register<IDbConnectionFactory>
                (new OrmLiteConnectionFactory(
                    ConfigurationManager.ConnectionStrings["PictureAuctionConnectionString"].ConnectionString,
                    SqlServerOrmLiteDialectProvider.Instance));

            SetConfig(new EndpointHostConfig
            {
#if DEBUG
                DebugMode = true,
                WebHostPhysicalPath = Path.GetFullPath(Path.Combine("~".MapServerPath(), "..", ".."))
#endif
            });

            JsConfig<DateTime>.SerializeFn = time => new DateTime(time.Ticks, DateTimeKind.Local).ToString("yyyy");

            Plugins.Add(new RazorFormat());
            Plugins.RemoveAll(x => x is MetadataFeature);
            Plugins.RemoveAll(x => x is RequestInfoFeature);
            Plugins.RemoveAll(x => x is CsvFormat);
            Plugins.RemoveAll(x => x is MarkdownFormat);

            SetConfig(new EndpointHostConfig
            {
                CustomHttpHandlers = {{HttpStatusCode.NotFound, new RazorHandler("/notfound")}}
            });
        }
    }
}