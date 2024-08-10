using NewsManagement.Entities.Tags;
using NewsManagement.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace NewsManagement.EntityRepositories.Tags
{
  public class EfCoreTagRepository : EfCoreRepository<NewsManagementDbContext, Tag, int>, ITagRepository
  {
    public EfCoreTagRepository(IDbContextProvider<NewsManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<List<Tag>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter = null)
    {
      var dbSet = await GetDbSetAsync();

      return await dbSet.WhereIf(!filter.IsNullOrWhiteSpace(), t => t.TagName.Contains(filter))
        .OrderBy(sorting).Skip(skipCount).Take(maxResultCount).ToListAsync();

    }
  }
}
