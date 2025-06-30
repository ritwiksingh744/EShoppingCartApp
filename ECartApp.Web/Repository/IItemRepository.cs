using ECartApp.Models;
using System.Linq.Expressions;

namespace ECartApp.Repository
{
    public interface IItemRepository : IGenericRepository<Items>
    {
        IQueryable<Items>  GetData(Expression<Func<Items, bool>> query);
    }
}
