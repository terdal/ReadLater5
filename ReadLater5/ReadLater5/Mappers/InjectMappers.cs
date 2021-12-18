using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCategoryProfile = Services.Mappers.CategoryProfile;
using BusinessBookmarkProfile = Services.Mappers.BookmarkProfile;
using Microsoft.Extensions.DependencyInjection;

namespace ReadLater5.Mappers
{
    public class InjectMappers
    {
        public static IMapper InjectMapperProfiles(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ResultProfile());
                mc.AddProfile(new CategoryProfile());
                mc.AddProfile(new BookmarkProfile());
                mc.AddProfile(new BusinessCategoryProfile());
                mc.AddProfile(new BusinessBookmarkProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            return mapper;
        }
    }
}
