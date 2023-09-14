using DoAn1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn1.IOFile
{
    public class IOFile
    {
        public void SaveProduct(Product product)
        {
            FileStream fs = new FileStream("E://data.txt", FileMode.Append, FileAccess.Write);
            StreamWriter w = new StreamWriter(fs);
            w.Write(product.productCode);// towis ddaay ne 
            w.Flush();
            fs.Close();
        }
    }
}
