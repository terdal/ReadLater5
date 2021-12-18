using Microsoft.AspNetCore.Mvc.Rendering;
using ReadLater5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadLater5.Helpers
{
    public static class MvcHelpers
    {
        public static SelectList GetBookmarkCompanySelectList(List<CategoryModel> PossibleCategories)
        {
            if (PossibleCategories == null)
            {
                PossibleCategories = new List<CategoryModel>() { };
            }
            return new SelectList(PossibleCategories, "ID", "Name");
        }
    }
}
