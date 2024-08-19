﻿using NewsManagement.Entities.Galleries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement.Entities.ListableContents
{
  public interface IListableContentGenericRepository<T> : IRepository<T, int>
    where T : ListableContent, new()
  {
    Task<List<T>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter);
  }
}
