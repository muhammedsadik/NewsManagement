﻿using Microsoft.EntityFrameworkCore;
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
  public class ListableContentCityRepository : EfCoreRepository<NewsManagementDbContext, ListableContentCity>, IListableContentCityRepository
  {
    public ListableContentCityRepository(IDbContextProvider<NewsManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<List<ListableContentCity>> GetCrossListAsync(int id)
    {
      var dbSet = await GetDbSetAsync();

      return await dbSet.Where(x => x.ListableContentId == id)
        .Include(x => x.City).ToListAsync();
    }
  }
}
