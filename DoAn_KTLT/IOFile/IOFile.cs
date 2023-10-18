using DoAn_KTLT.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DoAn_KTLT.IOFile
{
    public class IOFile
    {
        private static string? baseFolder = Environment.GetEnvironmentVariable("ASPNETCORE_FOLDER");
        private static string productFilePath = "product.json";
        private static string categoryFilePath = "category.json";
        private static string invoiceFilePath = "invoice.json";
        private static string goodsReceiptBillFilePath = "goodsReceiptBill.json";

        public static void checkFilePaths()
        {
            if (baseFolder == null)
            {
                return;
            }

            baseFolder = baseFolder.Trim();

            if (baseFolder == "")
            {
                return;

            }

            productFilePath = baseFolder + '/' + "product.json";
            categoryFilePath = baseFolder + '/' + "category.json";
            invoiceFilePath = baseFolder + '/' + "invoice.json";
            goodsReceiptBillFilePath = baseFolder + '/' + "goodsReceiptBill.json";

            if (!Directory.Exists(baseFolder))
            {
                Directory.CreateDirectory(baseFolder);
            }
        }

        // https://stackoverflow.com/questions/58003293/dotnet-core-system-text-json-unescape-unicode-string
        public static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        public static void Save<T>(string fileName, List<T> list)
        {
            checkFilePaths();
            string json = JsonSerializer.Serialize(list, jsonSerializerOptions);
            File.WriteAllText(fileName, json);
        }

        public static List<T> Load<T>(string fileName)
        {
            checkFilePaths();
            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                if (json != "")
                {
                    var list = JsonSerializer.Deserialize<List<T>>(json);
                    return list != null ? list : new List<T>();
                }
            }

            return new List<T>();
        }

        public static void SaveProducts(List<Product> products)
        {
            Save(productFilePath, products);
        }
        public static List<Product> ReadProduct()
        {
            return Load<Product>(productFilePath);
        }

        public static void SaveCategories(List<Category> categories)
        {
            Save(categoryFilePath, categories);
        }
        public static List<Category> ReadCategory()
        {
            return Load<Category>(categoryFilePath);
        }

        public static void SaveInvoices(List<Invoice> invoices)
        {
            Save(invoiceFilePath, invoices);
        }
        public static List<Invoice> ReadInvoice()
        {
            return Load<Invoice>(invoiceFilePath);
        }

        public static void SaveGoodsReceiptBills(List<GoodsReceiptBill> goodsReceiptBills)
        {
            Save(goodsReceiptBillFilePath, goodsReceiptBills);
        }
        public static List<GoodsReceiptBill> ReadGoodsReceiptBill()
        {
            return Load<GoodsReceiptBill>(goodsReceiptBillFilePath);
        }
    }
}