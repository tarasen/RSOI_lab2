using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PictureAuction.Api.ServiceModel.Types;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace PictureAuction.Api.ServiceModel.Routes
{
    public static class PictureRoutes
    {
        public class PictureDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime CreationDate { get; set; }

            public ICollection<CustomEntity> Artists { get; set; }
        }


        public class PictureExtendedDTO : PictureDTO
        {
            public string[] Genres { get; set; }
            public string Material { get; set; }
            public string Technique { get; set; }
            public string Gallery { get; set; }
            public double? Height { get; set; }
            public double? Width { get; set; }

            public string Image { get; set; }
        }


        [Route("/pictures", HttpMethods.Post)]
        public class CreatePicture : PictureExtendedDTO, IReturn<PictureExtendedDTO>
        {
        }


        [Route("/pictures/{Id}", HttpMethods.Delete)]
        public class DeletePicture : IReturnVoid
        {
            public int Id { get; set; }
        }


        [Route("/pictures/{Id}", HttpMethods.Get)]
        public class GetPicture : IReturn<PictureExtendedDTO>
        {
            public int Id { get; set; }
        }


        [Route("/pictures", HttpMethods.Get)]
        [DataContract]
        public class GetPictures : IReturn<PageResult<PictureDTO>>
        {
            [DataMember(Name = "page")]
            public int PageNumber { get; set; } = 1;

            [DataMember(Name = "page_size")]
            public int PageSize { get; set; } = 15;
        }


        [Route("/pictures/{Id}", HttpMethods.Put)]
        public class UpdatePicture : PictureExtendedDTO, IReturn<PictureExtendedDTO>
        {
        }
    }
}