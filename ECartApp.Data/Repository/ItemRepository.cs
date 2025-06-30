using ECartApp.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECartApp.Data.Repository
{
    public class ItemRepository : GenericRepository<Items>, IItemRepository
    {
        public ItemRepository(ECartContext context) : base(context)
        {
        }

        public IQueryable<Items> GetData(Expression<Func<Items, bool>> query)
        {
            return table.Where(query).Include(x=>x.SubCategory).Include(x=>x.SubCategory.Category);
        }
    }
}
