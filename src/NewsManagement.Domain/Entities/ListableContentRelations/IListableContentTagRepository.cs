using NewsManagement.Entities.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement.Entities.ListableContentRelations
{
  public interface IListableContentTagRepository : IRepository<ListableContentTag>
  {
    Task<List<ListableContentTag>> GetCrossListAsync(int id);
  }
}
