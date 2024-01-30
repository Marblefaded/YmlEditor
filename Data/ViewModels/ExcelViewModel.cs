using System.ComponentModel.DataAnnotations;

namespace YmlEditor.Data.ViewModels
{
    public class ExcelViewModel
    {
        public string Category { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string PopularProduct { get; set; }
        public string Price { get; set; }
        public string InStock { get; set; }
        public string Count { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Url { get; set; }
        public string Units { get; set; }
    }
}
