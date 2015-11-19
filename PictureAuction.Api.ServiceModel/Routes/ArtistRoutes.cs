using System.Collections.Generic;
using System.Runtime.Serialization;
using PictureAuction.Api.ServiceModel.Types;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace PictureAuction.Api.ServiceModel.Routes
{
    public static class ArtistRoutes
    {
        public class ArtistDTO
        {
            public int Id { get; set; }

            public string Name { get; set; }
            
            public ICollection<CustomEntity> Pictures { get; set; }
        }


        public class ArtistExtendedDTO : ArtistDTO
        {
            public string BirthDate { get; set; }

            public string DeathDate { get; set; }

            public string Nation { get; set; }

            public ICollection<string> Periods { get; set; }

        }


        [Route("/artists", HttpMethods.Post)]
        public class CreateArtist : ArtistExtendedDTO, IReturn<ArtistExtendedDTO>
        {
        }


        [Route("/artists/{Id}", HttpMethods.Delete)]
        public class DeleteArtist : IReturnVoid
        {
            public int Id { get; set; }
        }


        [Route("/artists/{Id}", HttpMethods.Get)]
        public class GetArtist : IReturn<ArtistExtendedDTO>
        {
            public int Id { get; set; }
        }


        [Route("/artists", HttpMethods.Get)]
        [DataContract]
        public class GetArtists : IReturn<PageResult<ArtistDTO>>
        {
            [DataMember(Name = "page")]
            public int PageNumber { get; set; } = 1;

            [DataMember(Name = "page_size")]
            public int PageSize { get; set; } = 15;
        }


        [Route("/artists/{Id}", HttpMethods.Put)]
        public class UpdateArtist : ArtistExtendedDTO, IReturn<ArtistExtendedDTO>
        {
        }

        [Route("/artists/{Id}/pictures", HttpMethods.Get)]
        public class GetArtistPictures : IReturn<PageResult<PictureRoutes.PictureDTO>>
        {
            public int Id { get; set; }

            [DataMember(Name = "page")]
            public int PageNumber { get; set; } = 1;

            [DataMember(Name = "page_size")]
            public int PageSize { get; set; } = 15;
        }
    }
}