using DoAn_KTLT.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DoAn_KTLT.IOFile
{
    public class IOFile
    {
        // https://stackoverflow.com/questions/58003293/dotnet-core-system-text-json-unescape-unicode-string
        public static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        public static void Save<T>(string fileName, List<T> list)
        {
            string json = JsonSerializer.Serialize(list, jsonSerializerOptions);
            File.WriteAllText(fileName, json);
        }

        public static List<T> Load<T>(string fileName)
        {
            var list = new List<T>();

            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                if (json != "" && json != null)
                {
                    return JsonSerializer.Deserialize<List<T>>(json);
                }
            }
            return list;
        }

        public static void SaveProducts(List<Product> products)
        {
            Save("product.json", products);
        }
        public static List<Product> ReadProduct()
        {
            return Load<Product>("product.json");
        }

        public static void SaveCategories(List<Category> categories)
        {
            Save("category.json", categories);
        }
        public static List<Category> ReadCategory()
        {
            return Load<Category>("category.json");
        }

        public static void SaveInvoices(List<Invoice> invoices)
        {
            Save("invoice.json", invoices);
        }
        public static List<Invoice> ReadInvoice()
        {
            return Load<Invoice>("invoice.json");
        }

        public static void SaveGoodsReceiptBills(List<GoodsReceiptBill> goodsReceiptBills)
        {
            Save("goodsReceiptBill.json", goodsReceiptBills);
        }
        public static List<GoodsReceiptBill> ReadGoodsReceiptBill()
        {
            return Load<GoodsReceiptBill>("goodsReceiptBill.json");
        }
    }
}
