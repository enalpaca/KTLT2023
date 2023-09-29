using DoAn_KTLT.IOFile;
using System;

namespace DoAn_KTLT.Models
{
    [Serializable]
    public class Product
    {
        public string ProductCode { get; set; } = "";
        public string ProductName { get; set; } = "";
        public DateTime ProductExpiredAt { get; set; }
        public string ProductCompany { get; set; } = "";
        public DateTime ProductProductionDate { get; set; }
        public string ProductCategory { get; set; } = "";
        public string ProductCategoryName { get; set; } = "";
        public long ProductPrice { get; set; } = 0;
        public int ProductQuantity { get; set; } = 0;

        public Product()
        {
            ProductCode = Utils.GenerateString();
        }
    }
}
