using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace NewsManagement.Entities.ListableContentBase
{
  public abstract class ListableContentBaseManager<TEntity, TKey> : DomainService
    where TEntity : class, IEntity, new()
  {


    public ListableContentBaseManager()
    {
      
    }



  }
}
