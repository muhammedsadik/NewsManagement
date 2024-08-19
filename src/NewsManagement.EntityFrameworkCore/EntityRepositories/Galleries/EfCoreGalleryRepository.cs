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
using NewsManagement.EntityRepositories.GenericRepository;
using Polly;
using NewsManagement.Entities.GenericRepository;

namespace NewsManagement.EntityRepositories.Galleries
{
  public class EfCoreGalleryRepository : GenericRepository<Gallery> , IGalleryRepository, IGenericRepository<Gallery>
  {
    public EfCoreGalleryRepository(IDbContextProvider<NewsManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    
  }
}
