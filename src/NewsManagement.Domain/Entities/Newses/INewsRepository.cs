using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement.Entities.Newses
{
  public interface INewsRepository : IRepository<News, int>
  {
    Task<List<News>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter = null);
  }
}
