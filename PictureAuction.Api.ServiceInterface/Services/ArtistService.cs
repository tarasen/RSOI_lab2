using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using MoreLinq;
using PictureAuction.Api.ServiceModel;
using PictureAuction.Api.ServiceModel.Routes;
using PictureAuction.Api.ServiceModel.Types;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace PictureAuction.Api.ServiceInterface.Services
{
    public class ArtistService : Service
    {
        static ArtistService()
        {
            Mapper.CreateMap<Artist, ArtistRoutes.ArtistExtendedDTO>()
                .ForMember(x => x.Name,
                    expression => expression.MapFrom(e => $"{e.SecondName}, {e.FirstName}"));
            Mapper.CreateMap<Artist, ArtistRoutes.ArtistDTO>()
                .ForMember(x => x.Name,
                    expression => expression.MapFrom(e => $"{e.SecondName}, {e.FirstName}"));
            Mapper.CreateMap<ArtistRoutes.ArtistExtendedDTO, Artist>()
                .ForMember(x => x.FirstName,
                    expression => expression.MapFrom(e => e.Name.Substring(0, e.Name.IndexOf(", ", StringComparison.Ordinal))))
                .ForMember(x => x.SecondName,
                    expression => expression.MapFrom(e => e.Name.Substring(e.Name.IndexOf(", ", StringComparison.Ordinal) + 2)));
            Mapper.CreateMap<Picture, PictureRoutes.PictureDTO>();
        }

        public object Get(ArtistRoutes.GetArtists request)
        {
            try
            {
                var count = Db.GetScalar<Artist, int>(r => Sql.Count(r.Id));
                var skip = (request.PageNumber - 1)*request.PageSize;

                if (skip >= count)
                    return new HttpResult(new PageResult<ArtistRoutes.ArtistDTO>(new ArtistRoutes.ArtistDTO[0], null, count), MimeTypes.Json);

                var artists = Db.Select<Artist>(q => q.Limit(skip, request.PageSize));
                var minId = artists.First().Id;
                var maxId = artists.Last().Id;

                var pba = Db.Select<PicturesByArtist>(p => p.ArtistId >= minId && p.ArtistId <= maxId)
                    .ToLookup(x => x.ArtistId, x => x.PictureId);

                var dto = new PageResult<ArtistRoutes.ArtistDTO>(
                    artists.Select(arg =>
                    {
                        var artistDto = Mapper.Map<ArtistRoutes.ArtistDTO>(arg);
                        artistDto.Pictures = pba.Where(x => x.Key == artistDto.Id).SelectMany(x => x).Select(x => new CustomEntity {Id = x}).ToArray();
                        return artistDto;
                    }),
                    new UriBuilder(Request.AbsoluteUri)
                    {
                        Query = $"page={request.PageNumber + 1}&page_size={request.PageSize}"
                    }.Uri,
                    count);

                return new HttpResult(dto, $"{MimeTypes.Json}; charset=utf-8");
            }
            catch
            {
                return new HttpError(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }

        public object Get(ArtistRoutes.GetArtistPictures request)
        {
            try
            {
                var artist = Db.GetByIdOrDefault<Artist>(request.Id);
                if (artist == null)
                    return HttpError.NotFound("Artist with this id is not found");

                var pbta = Db.Select<PicturesByArtist>(x => x.ArtistId == artist.Id);
                var skip = (request.PageNumber - 1)*request.PageSize;

                if (skip >= pbta.Count)
                    return new HttpResult(new PageResult<PictureRoutes.PictureDTO>(new PictureRoutes.PictureDTO[0], null, pbta.Count), MimeTypes.Json);

                var pictures = Db.Select<Picture>().Join(pbta, p => p.Id, p => p.PictureId, (p, _) => p).Skip(skip).Take(request.PageSize).ToList(); //.Limit(skip, request.PageSize));
                var minId = pictures.First().Id;
                var maxId = pictures.Last().Id;

                var pba =
                    Db.Select<PicturesByArtist>(p => p.PictureId >= minId && p.PictureId <= maxId)
                        .Join(pictures, p => p.PictureId, p => p.Id, (p, _) => p)
                        .ToLookup(x => x.PictureId, x => x.ArtistId);

                var dto = new PageResult<PictureRoutes.PictureDTO>(
                    pictures.Select(p =>
                    {
                        var pictureDto = Mapper.Map<PictureRoutes.PictureDTO>(p);
                        pictureDto.Artists = pba.Where(x => x.Key == p.Id).SelectMany(x => x).Select(x => new CustomEntity {Id = x}).ToArray();
                        return pictureDto;
                    }),
                    new UriBuilder(Request.AbsoluteUri) {Query = $"page={request.PageNumber + 1}&page_size={request.PageSize}"}.Uri,
                    pbta.Count);

                return new HttpResult(dto, $"{MimeTypes.Json}; charset=utf-8");
            }
            catch
            {
                return new HttpError(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }

        public object Get(ArtistRoutes.GetArtist request)
        {
            try
            {
                var artist = Db.GetByIdOrDefault<Artist>(request.Id);
                if (artist == null)
                    return HttpError.NotFound("Artist with this id is not found");

                var dto = Mapper.Map<ArtistRoutes.ArtistExtendedDTO>(artist);
                dto.Pictures = Db.Select<PicturesByArtist>(x => x.ArtistId == artist.Id).Select(x => new CustomEntity {Id = x.PictureId}).ToArray();
                dto.Nation = Db.GetByIdOrDefault<Nation>(artist.NationId)?.Name;
                dto.Periods = Db.Select<ArtistsByPeriod>(x => x.ArtistId == artist.Id)
                    .Join(Db.Select<Period>(), pb => pb.PeriodId, g => g.Id, (_, g) => g.Name).ToArray();
                return new HttpResult(dto, $"{MimeTypes.Json}; charset=utf-8");
            }
            catch
            {
                return new HttpError(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }

        public object Post(ArtistRoutes.CreateArtist request)
        {
            try
            {
                var token = AuthService.CheckPermitions(Request.Headers["Authorization"], Db);
                if (token == null)
                    return HttpError.Unauthorized("Please, authorize");

                var art = Mapper.Map<Artist>(request);
                art.NationId = Db.Select<Nation>(x => x.Name == request.Nation).First().Id;

                Db.Insert(art);
                var id = Db.GetScalar<Artist, int>(r => Sql.Max(r.Id));
                var artist = Db.GetById<Artist>(id);

                if (request.Pictures.Any())
                {
                    var pba = Db.Select<PicturesByArtist>().Join(request.Pictures, x => x.PictureId, a => a.Id, (x, _) => x).ToList();

                    Db.DeleteAll(pba);

                    Db.SaveAll(pba.Select(x => new PicturesByArtist {PictureId = x.PictureId, ArtistId = artist.Id}));
                    Db.SaveAll(request.Pictures.Select(x => new PicturesByArtist {PictureId = x.Id, ArtistId = artist.Id}).ExceptBy(pba, o => o.PictureId).ToList());
                }

                if (request.Periods.Any())
                {
                    var abp = Db.Select<Period>()
                        .Join(request.Periods, x => x.Name, x => x, (x, _) => x.Id)
                        .Join(Db.Select<ArtistsByPeriod>(), x => x, x => x.PeriodId, (_, p) => p)
                        .ToList();
                    
                    Db.DeleteAll(abp);
                    Db.SaveAll(abp.Select(x => new ArtistsByPeriod {PeriodId = x.PeriodId, ArtistId = artist.Id}));
                }

                return new HttpResult(artist, $"{MimeTypes.Json}; charset=utf-8") {StatusCode = HttpStatusCode.Created};
            }
            catch (InvalidOperationException e)
            {
                return new HttpError(HttpStatusCode.BadRequest, "Wrong Parameters");
            }
            catch
            {
                return new HttpError(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }

        public object Put(ArtistRoutes.UpdateArtist request)
        {
            try
            {
                var token = AuthService.CheckPermitions(Request.Headers["Authorization"], Db);
                if (token == null)
                    return HttpError.Unauthorized("Please, authorize");

                var artist = Db.GetByIdOrDefault<Artist>(request.Id);
                if (artist == null)
                    return HttpError.NotFound($"Artist '{request.Id}' does not exist");

                Mapper.Map(request, artist, o => o.AfterMap((rq, art) => art.Id = rq.Id));
                artist.NationId = Db.Select<Nation>(x => x.Name == request.Nation).First().Id;
                Db.Update(artist);

                if (request.Pictures.Any())
                {
                    var pba = Db.Select<PicturesByArtist>().Join(request.Pictures, x => x.PictureId, a => a.Id, (x, _) => x).ToList();

                    Db.DeleteAll(pba);

                    Db.SaveAll(pba.Select(x => new PicturesByArtist {PictureId = x.PictureId, ArtistId = artist.Id}));
                    Db.SaveAll(request.Pictures.Select(x => new PicturesByArtist { PictureId = x.Id, ArtistId = artist.Id }).ExceptBy(pba, o => o.PictureId).ToList());
                }
                if (request.Periods.Any())
                {
                    var abp = Db.Select<Period>()
                        .Join(request.Periods, x => x.Name, x => x, (x, _) => x.Id)
                        .Join(Db.Select<ArtistsByPeriod>(), x => x, x => x.PeriodId, (_, p) => p)
                        .ToList();

                    Db.DeleteAll(abp);
                    Db.SaveAll(abp.Select(x => new ArtistsByPeriod {PeriodId = x.PeriodId, ArtistId = artist.Id}));
                }

                return new HttpResult(artist, $"{MimeTypes.Json}; charset=utf-8")
                {
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch
            {
                return new HttpError(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }

        public object Delete(ArtistRoutes.DeleteArtist request)
        {
            try
            {
                var token = AuthService.CheckPermitions(Request.Headers["Authorization"], Db);
                if (token == null)
                    return HttpError.Unauthorized("Please, authorize");

                if (Db.Any<PicturesByArtist>(x => x.ArtistId == request.Id))
                    return HttpError.Conflict("Can't remove artist with existing pictures");

                var deleted = Db.Delete<ArtistsByPeriod>(x => x.ArtistId == request.Id)
                              + Db.Delete<Artist>(x => x.Id == request.Id);

                return deleted == 0
                    ? HttpError.NotFound("")
                    : new HttpError(HttpStatusCode.NoContent, "");
            }
            catch
            {
                return new HttpError(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
    }
}