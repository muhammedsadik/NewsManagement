using Microsoft.EntityFrameworkCore;
using NewsManagement.Entities.Galleries;
using NewsManagement.Entities.ListableContents;
using NewsManagement.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NewsManagement.EntityRepositories.ListableContents
{
    public class ListableContentGenericRepository<T> : EfCoreRepository<NewsManagementDbContext, T, int>, IListableContentGenericRepository<T>
      where T : ListableContent, new()
    {

        public ListableContentGenericRepository(IDbContextProvider<NewsManagementDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<T>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter)
        {

            var dbSet = await GetDbSetAsync();

            return await dbSet.WhereIf(!filter.IsNullOrWhiteSpace(), c => c.Title.Contains(filter))
                .OrderBy(sorting).Skip(skipCount).Take(maxResultCount).ToListAsync();

        }
    }
}