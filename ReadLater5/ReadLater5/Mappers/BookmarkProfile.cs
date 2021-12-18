using AutoMapper;
using ReadLater5.Models;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadLater5.Mappers
{
    public class BookmarkProfile : Profile
    {
        public BookmarkProfile()
        {
            CreateMap<BookmarkDTO, BookmarkModel>()
                .ForMember(x => x.CreateDate, y => y.MapFrom(z => TimeZoneInfo.ConvertTimeFromUtc(z.CreateDate, TimeZoneInfo.Local).ToShortDateString()))
                .ReverseMap()
                .ForMember(x => x.CreateDate, y => y.Ignore());
            CreateMap<UpsertBookmarkDTO, UpsertBookmarkModel>()
                .ForMember(x => x.PossibleCategories, y => y.Ignore())
                .ForMember(x => x.Categories, y => y.Ignore())
                .ReverseMap();
            CreateMap<BookmarkDTO, UpsertBookmarkModel>()
                .ForMember(x => x.PossibleCategories, y => y.Ignore())
                .ForMember(x => x.Categories, y => y.Ignore())
                ;
        }
    }
}
