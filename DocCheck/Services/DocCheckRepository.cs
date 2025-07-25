using DocCheck.Data;
using DocCheck.Models;
using Microsoft.EntityFrameworkCore;

namespace DocCheck.Services
{
    public class DocCheckRepository(ApplicationDbContext dbContext)
    {
        public async Task<DocumentCheck[]> GetItemsAsync(SearchParams searchParams)
        {
            var result = await dbContext.DocumentCheck
                .AsNoTracking()
                .HandleQuery(searchParams)
                .ToArrayAsync();

            return result;
        }

        public async Task<int> GetCountAsync(SearchParams searchParams)
        {
            var result = await dbContext.DocumentCheck
                .AsNoTracking()
                .HandleFilterQuery(searchParams)
                .CountAsync();

            return result;
        }

        public async Task<DocumentCheck?> GetItemAsync(Guid id, bool isIncludeErrors = false)
        {
            var query = dbContext.DocumentCheck
                .AsNoTracking();

            if (isIncludeErrors)
                query = query.Include(e => e.Errors);

            var result = await query.FirstOrDefaultAsync(e => e.Id == id);

            return result;
        }

        public async Task<DocumentCheck?> GetItemAsync(string refKey, bool isIncludeErrors = false)
        {
            var query = dbContext.DocumentCheck
                .AsNoTracking();

            if (isIncludeErrors)
                query = query.Include(e => e.Errors);

            var result = await query.FirstOrDefaultAsync(e => e.InvoiceRefKey == refKey);

            return result;
        }

        public async Task<bool> ExistsAsync(string refKey)
        {
            var result = await dbContext.DocumentCheck.AsNoTracking().AnyAsync(e => e.InvoiceRefKey == refKey);

            return result;
        }

        public async Task CreateAsync(DocumentCheck item)
        {
            item.CreatedAt = DateTime.Now;

            dbContext.DocumentCheck.Add(item);

            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(DocumentCheck item)
        {
            var existing = await dbContext.DocumentCheck.FirstOrDefaultAsync(e => e.Id == item.Id);

            if (existing == null)
                return;

            dbContext.Entry(existing).CurrentValues.SetValues(item);

            existing.UpdatedAt = DateTime.Now;

            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await dbContext.DocumentCheck.FirstOrDefaultAsync(e => e.Id == id);

            if (existing == null)
                return;

            dbContext.Remove(existing);

            await dbContext.SaveChangesAsync();
        }
    }
}
