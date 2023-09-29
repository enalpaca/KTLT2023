using DoAn_KTLT.IOFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn_KTLT.Models
{

    [Serializable]
    public class Category
    {
        public string categoryCode { get; set; } = "";
        public string categoryName { get; set; } = "";

        public Category()
        {
            categoryCode = Utils.GenerateString();
        }

    }
}
