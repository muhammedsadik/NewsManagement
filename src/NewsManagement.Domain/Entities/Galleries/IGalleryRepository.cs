using NewsManagement.Entities.GenericRepository;
using NewsManagement.Entities.ListableContents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement.Entities.Galleries
{
  public interface IGalleryRepository : IGenericRepository<Gallery>
  {
  }
}
