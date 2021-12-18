using AutoMapper;
using Data;
using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.DTOs;
using Services.Helpers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private ReadLaterDataContext _ReadLaterDataContext;
        private readonly ILogger<CategoryService> _logger;
        private readonly IMapper _mapper;

        public CategoryService(ReadLaterDataContext readLaterDataContext, ILogger<CategoryService> logger, IMapper mapper) 
        {
            _ReadLaterDataContext = readLaterDataContext;
            _logger = logger;
            _mapper = mapper;
        }

        public ResultDTO<CategoryDTO> CreateCategory(CategoryDTO category, string userId)
        {
            var result = new ResultDTO<CategoryDTO>();
            try
            {
                var dbCategory = new Category()
                {
                    Name = category.Name,
                    UserId = userId
                };

                _ReadLaterDataContext.Add(dbCategory);
                _ReadLaterDataContext.SaveChanges();
                result.Data = _mapper.Map<CategoryDTO>(dbCategory);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} Exception: {1}", ex.Message, ex);
                result.ErrorMessage = CommonHelpers.GetGeneralErrorMessage();
            }
            return result;
        }

        public ResultDTO UpdateCategory(CategoryDTO category, string userId)
        {
            var result = new ResultDTO();
            try
            {
                var dbCategory = _ReadLaterDataContext.Categories.Where(c => c.ID == category.ID && c.UserId == userId).FirstOrDefault();
                if (dbCategory == null)
                {
                    result.ErrorMessage = $"Category with id: {category.ID} doesn't exist";
                    return result;
                }

                dbCategory.Name = category.Name;

                _ReadLaterDataContext.Update(dbCategory);
                _ReadLaterDataContext.SaveChanges();
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} Exception: {1}", ex.Message, ex);
                result.ErrorMessage = CommonHelpers.GetGeneralErrorMessage();
            }
            return result;
        }

        public List<CategoryDTO> GetCategories(string userId)
        {
            var categories = new List<CategoryDTO>();
            try
            {
                var dbCategories = _ReadLaterDataContext.Categories.Where(x => x.UserId == userId).ToList();
                categories = _mapper.Map<List<CategoryDTO>>(dbCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} Exception: {1}", ex.Message, ex);
            }
            return categories;
        }

        public CategoryDTO GetCategory(int Id, string userId)
        {
            CategoryDTO category = null;
            try
            {
                var dbCategory = _ReadLaterDataContext.Categories.Where(c => c.ID == Id && c.UserId == userId).FirstOrDefault();
                category = _mapper.Map<CategoryDTO>(dbCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} Exception: {1}", ex.Message, ex);
            }

            return category;
        }

        public CategoryDTO GetCategory(string Name, string userId)
        {
            CategoryDTO category = null;
            try
            {
                var dbCategory = _ReadLaterDataContext.Categories.Where(c => c.Name.ToLower() == Name.ToLower() && c.UserId == userId).FirstOrDefault();
                category = _mapper.Map<CategoryDTO>(dbCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} Exception: {1}", ex.Message, ex);
            }

            return category;
        }

        public ResultDTO DeleteCategory(int categoryId, string userId)
        {
            var result = new ResultDTO();
            try
            {
                var dbCategory = _ReadLaterDataContext.Categories.Where(c => c.ID == categoryId && c.UserId == userId).FirstOrDefault();

                if (dbCategory == null)
                {
                    result.ErrorMessage = $"Category with id: {categoryId} doesn't exist";
                    return result;
                }

                _ReadLaterDataContext.Categories.Remove(dbCategory);
                _ReadLaterDataContext.SaveChanges();

                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} Exception: {1}", ex.Message, ex);
                result.ErrorMessage = CommonHelpers.GetGeneralErrorMessage();
            }
            return result;
        }
    }
}
