using Microsoft.AspNetCore.Mvc.Rendering;
using ReadLater5.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReadLater5.Models
{
    public class BookmarkModel
    {
        public int ID { get; set; }

        [StringLength(maximumLength: 500)]
        public string URL { get; set; }

        public string ShortDescription { get; set; }

        public int? CategoryId { get; set; }

        public virtual CategoryModel Category { get; set; }

        public string CreateDate { get; set; }
    }

    public class UpsertBookmarkModel
    {
        public int ID { get; set; }

        [StringLength(maximumLength: 500)]
        public string URL { get; set; }

        public string ShortDescription { get; set; }

        public int? CategoryId { get; set; }

        [Display(Name = "New Category")]
        public string CategoryName { get; set; }

        public List<CategoryModel> PossibleCategories { get; set; }

        [Display(Name = "Categories")]
        public IEnumerable<SelectListItem> Categories
        {
            get
            {
                return MvcHelpers.GetBookmarkCompanySelectList(PossibleCategories);
            }
            set { }
        }

        public UpsertBookmarkModel()
        {
            PossibleCategories = new List<CategoryModel>();
        }
    }
}
