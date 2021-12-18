using AutoMapper;
using ReadLater5.Models;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadLater5.Mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDTO, CategoryModel>().ReverseMap();
        }
    }
}
