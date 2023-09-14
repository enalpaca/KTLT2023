using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn1.Models
{
    public class Product
    {
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productExpiredAt { get; set; }
       
    }
    public class ProductViewModel
    {
        public Product[] productList = {};

    }
}
