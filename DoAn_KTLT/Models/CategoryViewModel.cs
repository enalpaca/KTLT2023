using DoAn_KTLT.IOFile;

namespace DoAn_KTLT.Models
{

    [Serializable]
    public class Category
    {
        public string CategoryCode { get; set; } = "";
        public string CategoryName { get; set; } = "";

        public Category()
        {
            CategoryCode = Utils.GenerateString();
        }

    }
}
