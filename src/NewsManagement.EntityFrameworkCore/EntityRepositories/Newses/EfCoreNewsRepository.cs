using Microsoft.EntityFrameworkCore;
using NewsManagement.Entities.ListableContents;
using NewsManagement.Entities.Newses;
using NewsManagement.EntityFrameworkCore;
using NewsManagement.EntityRepositories.ListableContents;
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
  public class EfCoreNewsRepository : ListableContentGenericRepository<News> , INewsRepository, IListableContentGenericRepository<News>
  {
    public EfCoreNewsRepository(IDbContextProvider<NewsManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }


  }
}
