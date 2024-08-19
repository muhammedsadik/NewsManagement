using Microsoft.EntityFrameworkCore;
using NewsManagement.Entities.Newses;
using NewsManagement.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NewsManagement.EntityRepositories.Newses
{
  public class EfCoreNewsRepository : EfCoreRepository<NewsManagementDbContext, News, int>, INewsRepository
  {
    public EfCoreNewsRepository(IDbContextProvider<NewsManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<List<News>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter = null)
    {
      var dbSet = await GetDbSetAsync();

      return await dbSet.WhereIf(!filter.IsNullOrWhiteSpace(), c => c.Title.Contains(filter))
        .OrderBy(sorting).Skip(skipCount).Take(maxResultCount).ToListAsync();
    }
  }
}
