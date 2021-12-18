using AutoMapper;
using ReadLater5API.Models;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadLater5API.Mappers
{
    public class BookmarkProfile : Profile
    {
        public BookmarkProfile()
        {
            CreateMap<BookmarkDTO, BookmarkModel>()
                .ForMember(x => x.CreateDate, y => y.MapFrom(z => TimeZoneInfo.ConvertTimeFromUtc(z.CreateDate, TimeZoneInfo.Local).ToShortDateString()))
                .ReverseMap()
                .ForMember(x => x.CreateDate, y => y.Ignore());
            CreateMap<UpsertBookmarkDTO, UpsertBookmarkModel>().ReverseMap();
            CreateMap<BookmarkDTO, UpsertBookmarkModel>();
        }
    }
}
