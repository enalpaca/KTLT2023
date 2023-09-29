using DoAn_KTLT.IOFile;

namespace DoAn_KTLT.Models
{
    [Serializable]
    public class Invoice
    {
        public string invoiceCode { get; set; } = "";
        public string invoiceCreateDate { get; set; } = "";
        public string invoiceCustomerName { get; set; } = "";
        public string invoiceCustomerAddress { get; set; } = "";
        public string invoiceCustomerPhone { get; set; } = "";
        public string invoiceProductname { get; set; } = "";
        public string invoiceProductCode { get; set; } = "";
        public string invoiceProductPrice { get; set; } = "";
        public string invoiceProductQuantity { get; set; } = "";
        public Invoice()
        {
            invoiceCode = Utils.GenerateString();
        }

    }
}
