using NewsManagement.Entities.Galleries;
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
using Polly;
using NewsManagement.EntityRepositories.ListableContents;
using NewsManagement.Entities.ListableContents;

namespace NewsManagement.EntityRepositories.Galleries
{
  public class EfCoreGalleryRepository : ListableContentGenericRepository<Gallery>, IGalleryRepository, IListableContentGenericRepository<Gallery>
  {
    public EfCoreGalleryRepository(IDbContextProvider<NewsManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }


  }
}
