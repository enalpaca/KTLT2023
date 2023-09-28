using DoAn_KTLT.IOFile;
using System;

namespace DoAn_KTLT.Models
{
    [Serializable]
    public class Product
    {
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productExpiredAt { get; set; }
        public string productCompany { get; set; }
        public string productProductionDate { get; set; }
        public string productCategory { get; set; }
        public string productPrice { get; set; }
        public string productQuantity { get; set; }

        public Product()
        {
            productCode = Utils.GenerateString();
        }
    }
}
