using DocCheck.Common;
using DocCheck.Data;
using Microsoft.EntityFrameworkCore;

namespace DocCheck.Services
{
    public class Repository<TEntity>(ApplicationDbContext dbContext) 
        where TEntity : class , IHasId, IHasDocument
    {
        public async Task<TEntity[]> GetValuesAsync(SearchParams searchParams)
        {
            var result = await dbContext
                .Set<TEntity>()
                .AsNoTracking()
                .HandleQuery(searchParams)
                .ToArrayAsync();

            return result;
        }

        public async Task<int> GetCountAsync(SearchParams searchParams)
        {
            var result = await dbContext
                .Set<TEntity>()
                .AsNoTracking()
                .HandleFilterQuery(searchParams)
                .CountAsync();

            return result;
        }

        public async Task<TEntity?> GetValueAsync(Guid id)
        {
            var result = await dbContext
                .Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            return result;
        }
    }
}
