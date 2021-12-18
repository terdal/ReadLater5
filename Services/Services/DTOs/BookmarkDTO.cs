using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class BookmarkDTO
    {
        public int ID { get; set; }

        public string URL { get; set; }

        public string ShortDescription { get; set; }

        public int? CategoryId { get; set; }

        public CategoryDTO Category { get; set; }

        public DateTime CreateDate { get; set; }
    }

    public class UpsertBookmarkDTO
    {
        public int ID { get; set; }

        public string URL { get; set; }

        public string ShortDescription { get; set; }

        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
