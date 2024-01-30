using System.ComponentModel.DataAnnotations;

namespace YmlEditor.Data.ViewModels
{
    public class ViewModel
    {
        /*[Required(ErrorMessage = "First Name is mandatory")]
        [MinLength(2)]*/

        [Required(ErrorMessage = "Category is mandatory")]
        [MinLength(2)]
        public string Category { get; set; }
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is mandatory")]
        [MinLength(2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "PopularProduct is mandatory")]
        [MinLength(2)]
        public string PopularProduct { get; set; }

        [Required(ErrorMessage = "Price is mandatory")]
        [MinLength(2)]
        public string Price { get; set; }

        [Required(ErrorMessage = "InStock is mandatory")]
        [MinLength(2)]
        public string InStock { get; set; }

        [Required(ErrorMessage = "Count is mandatory")]
        [MinLength(2)]
        public string Count { get; set; }

        [Required(ErrorMessage = "Picture is mandatory")]
        [MinLength(2)]
        public string Picture { get; set; }

        [Required(ErrorMessage = "Description is mandatory")]
        [MinLength(2)]
        public string Description { get; set; }

        [Required(ErrorMessage = "ShortDescription is mandatory")]
        [MinLength(2)]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Url is mandatory")]
        [MinLength(2)]
        public string Url { get; set; }
        [Required(ErrorMessage = "Units is mandatory")]
        [MinLength(2)]
        public string Units { get; set; }
    }
}
