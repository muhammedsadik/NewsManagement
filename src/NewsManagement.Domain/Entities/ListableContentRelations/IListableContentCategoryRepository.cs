using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement.Entities.ListableContentRelations
{
  public interface IListableContentCategoryRepository : IRepository<ListableContentCategory>
  {
    Task<List<ListableContentCategory>> GetCrossListAsync(int id);
  }
}
