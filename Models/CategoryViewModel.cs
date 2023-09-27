using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn1.Models
{
    public class Category
    {
        public string categoryCode { get; set; }
        public string categoryName { get; set; }

    }

    public class CategoryViewModel
    {
        public Category[] categoryList = {};
    }
}
