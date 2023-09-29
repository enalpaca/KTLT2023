using DoAn_KTLT.IOFile;

namespace DoAn_KTLT.Models
{
    [Serializable]
    public class InvoiceProduct
    {
        public string InvoiceProductName { get; set; } = "";
        public string InvoiceProductCode { get; set; } = "";
        public long InvoiceProductPrice { get; set; } = 0;
        public int InvoiceProductQuantity { get; set; } = 0;
    }

    [Serializable]
    public class Invoice
    {
        public string InvoiceCode { get; set; } = "";
        public DateTime InvoiceCreateDate { get; set; }
        public string InvoiceCustomerName { get; set; } = "";
        public string InvoiceCustomerAddress { get; set; } = "";
        public string InvoiceCustomerPhone { get; set; } = "";
        public List<InvoiceProduct> PoductItems { get; set; } = new List<InvoiceProduct>();
        public Invoice()
        {
            InvoiceCode = Utils.GenerateString();
        }
    }
}
