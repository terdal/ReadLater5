using AutoMapper;
using ReadLater5API.Models;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadLater5API.Mappers
{
    public class ResultProfile : Profile
    {
        public ResultProfile()
        {
            CreateMap<ResultDTO, ResultModel>().ReverseMap();
        }
    }
}
