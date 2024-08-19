using Microsoft.EntityFrameworkCore;
using NewsManagement.Entities.Videos;
using NewsManagement.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NewsManagement.EntityRepositories.Videos
{
  public class EfCoreVideoRepository : EfCoreRepository<NewsManagementDbContext, Video, int>, IVideoRepository
  {
    public EfCoreVideoRepository(IDbContextProvider<NewsManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<List<Video>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter = null)
    {
      var dbSet = await GetDbSetAsync();

      return await dbSet.WhereIf(!filter.IsNullOrWhiteSpace(), c => c.Title.Contains(filter))
        .OrderBy(sorting).Skip(skipCount).Take(maxResultCount).ToListAsync();
    }
  }
}
