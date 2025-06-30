using ECartApp.Data.Entity;
using System.Linq.Expressions;

namespace ECartApp.Data.Repository
{
    public interface IItemRepository : IGenericRepository<Items>
    {
        IQueryable<Items>  GetData(Expression<Func<Items, bool>> query);
    }
}
