using System.ComponentModel.DataAnnotations;

namespace ECartApp.Data.Entity
{
    public class SubCategory
    {
        [Key]
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; } 
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public IEnumerable<Items> Items { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime? UpdatedOn { get; set; } = null;
        public bool IsDeleted { get; set; } = false;

    }
}
