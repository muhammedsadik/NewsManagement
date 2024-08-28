using NewsManagement.Entities.ListableContents;
using NewsManagement.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NewsManagement.EntityRepositories.ListableContents
{
  public class EfCoreListableContentRepository : EfCoreRepository<NewsManagementDbContext, ListableContent, int>, IListableContentRepository
  {
    public EfCoreListableContentRepository(IDbContextProvider<NewsManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }







  }
}
