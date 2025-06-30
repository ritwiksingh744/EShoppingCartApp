using System.ComponentModel.DataAnnotations;

namespace ECartApp.Models.Item
{
    public class ItemAddModel
    {
        [Required(ErrorMessage = "Name is reqired.")]
        public string ItemName { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Subcategory is required.")]
        public int SubCategoryId { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        public double Price { get; set; }
        [DataType(DataType.Url)]
        [Required(ErrorMessage = "Image url is required.")]
        public string ImageUrl { get; set; }
    }
}
