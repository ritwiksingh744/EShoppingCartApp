using System.ComponentModel.DataAnnotations;

namespace ECartApp.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please enter Category Name.")]
        public string CategoryName { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime? UpdatedOn { get; set; } = null;
        public bool IsDeleted { get; set; } = false;

        public IEnumerable<SubCategory> SubCategories { get; set; }
    }
}
