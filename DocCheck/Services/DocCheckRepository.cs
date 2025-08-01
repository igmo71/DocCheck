﻿using DocCheck.Data;
using DocCheck.Models;
using Microsoft.EntityFrameworkCore;

namespace DocCheck.Services
{
    public class DocCheckRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        public async Task<DocumentCheck[]> GetItemsAsync(SearchParams searchParams)
        {
            using var dbContext = dbContextFactory.CreateDbContext();

            var result = await dbContext.DocumentCheck
                .AsNoTracking()
                .HandleQuery(searchParams)
                .ToArrayAsync();

            return result;
        }

        public async Task<int> GetCountAsync(SearchParams searchParams)
        {
            using var dbContext = dbContextFactory.CreateDbContext();

            var result = await dbContext.DocumentCheck
                .AsNoTracking()
                .HandleFilterQuery(searchParams)
                .CountAsync();

            return result;
        }

        public async Task<DocumentCheck?> GetItemAsync(string invoiceRefKey, bool isIncludeErrors = false)
        {
            using var dbContext = dbContextFactory.CreateDbContext();

            var query = dbContext.DocumentCheck
                .AsNoTracking();

            if (isIncludeErrors)
                query = query.Include(e => e.Errors);

            var result = await query.FirstOrDefaultAsync(e => e.InvoiceRefKey == invoiceRefKey);

            return result;
        }

        public async Task<bool> ExistsAsync(string refKey)
        {
            using var dbContext = dbContextFactory.CreateDbContext();

            var result = await dbContext.DocumentCheck.AsNoTracking().AnyAsync(e => e.InvoiceRefKey == refKey);

            return result;
        }

        public async Task CreateAsync(DocumentCheck item)
        {
            using var dbContext = dbContextFactory.CreateDbContext();

            item.CreatedAt = DateTime.Now;

            dbContext.DocumentCheck.Add(item);

            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(DocumentCheck item)
        {
            using var dbContext = dbContextFactory.CreateDbContext();

            var existing = await dbContext.DocumentCheck
                .Include(e => e.Errors)
                .FirstOrDefaultAsync(e => e.Id == item.Id);

            if (existing == null)
                return;

            AddLog(dbContext, existing);

            dbContext.Entry(existing).CurrentValues.SetValues(item);

            existing.Errors = item.Errors;

            existing.UpdatedAt = DateTime.Now;

            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var dbContext = dbContextFactory.CreateDbContext();

            var existing = await dbContext.DocumentCheck.FirstOrDefaultAsync(e => e.Id == id);

            if (existing == null)
                return;

            AddLog(dbContext, existing);

            dbContext.Remove(existing);

            await dbContext.SaveChangesAsync();
        }

        private static void AddLog(ApplicationDbContext dbContext, DocumentCheck existing)
        {
            var log = new DocumentCheckLog(existing);
            dbContext.DocumentCheckLog.Add(log);
        }

        public async Task DeleteAsync(string refKey)
        {
            using var dbContext = dbContextFactory.CreateDbContext();

            var existing = await dbContext.DocumentCheck.FirstOrDefaultAsync(e => e.InvoiceRefKey == refKey);

            if (existing == null)
                return;

            dbContext.Remove(existing);

            await dbContext.SaveChangesAsync();
        }
    }
}
