using Microsoft.EntityFrameworkCore;
using NewsManagement.Entities.ListableContentRelations;
using NewsManagement.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NewsManagement.EntityRepositories.ListableContentRelations
{
  public class ListableContentCategoryRepository : EfCoreRepository<NewsManagementDbContext, ListableContentCategory>, IListableContentCategoryRepository
  {
    public ListableContentCategoryRepository(IDbContextProvider<NewsManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<List<ListableContentCategory>> GetCrossListAsync(int id)
    {
      var dbSet = await GetDbSetAsync(); 

      return await dbSet.Where(x => x.ListableContentId == id)
        .Include(x => x.Category).ToListAsync();
    }
  }
}
