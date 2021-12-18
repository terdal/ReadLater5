using AutoMapper;
using Entity;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class BookmarkProfile : Profile
    {
        public BookmarkProfile()
        {
            CreateMap<Bookmark, BookmarkDTO>();
        }
    }
}
