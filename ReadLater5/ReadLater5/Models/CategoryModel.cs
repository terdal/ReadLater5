using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReadLater5.Models
{
    public class CategoryModel
    {
        public int ID { get; set; }

        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
    }
}
