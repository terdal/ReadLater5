using Entity;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICategoryService
    {
        ResultDTO<CategoryDTO> CreateCategory(CategoryDTO category, string userId);
        List<CategoryDTO> GetCategories(string userId);
        CategoryDTO GetCategory(int Id, string userId);
        CategoryDTO GetCategory(string Name, string userId);
        ResultDTO UpdateCategory(CategoryDTO category, string userId);
        ResultDTO DeleteCategory(int categoryId, string userId);
    }
}
