using DoAn1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DoAn1.IOFile
{
    public class IOFile
    {
        public static void SaveProduct(Product product)
        {
            FileStream fs = new FileStream("product.txt", FileMode.Append, FileAccess.Write);
            StreamWriter w = new StreamWriter(fs);
            string json = JsonSerializer.Serialize(product);
            w.WriteLine(json);
            w.Flush();
            fs.Close();

        }

        public static List<Product> ReadProduct()
        {

            List<Product> readProductList = new List<Product>();
            var lines = File.ReadLines("product.txt");
            foreach (var line in lines)
            {
                Product product = JsonSerializer.Deserialize<Product>(line);
                readProductList.Add(product);
            }
            return readProductList;
        }
    }
}
