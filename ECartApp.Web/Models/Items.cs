using System.ComponentModel.DataAnnotations;

namespace ECartApp.Models
{
    public class Items
    {
        [Key]
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public double Price { get; set; }
        [DataType(DataType.Url)]
        public string ImageUrl { get; set; }
        public virtual SubCategory SubCategory { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime? UpdatedOn { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
    }
}
