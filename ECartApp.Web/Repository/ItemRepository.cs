using ECartApp.DAL;
using ECartApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;

namespace ECartApp.Repository
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
