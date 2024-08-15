using NewsManagement.Entities.Galleries;
using NewsManagement.EntityDtos.GalleryDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement.AppService.Galleries
{
  public class GalleryAppService : CrudAppService<Gallery, GalleryDto, int, GetListPagedAndSortedDto, CreateGalleryDto, UpdateGalleryDto>, IGalleryAppService
  {
    public GalleryAppService(IRepository<Gallery, int> repository) : base(repository)
    {
    }

    public Task DeleteHardAsync(int id)
    {
      throw new NotImplementedException();
    }
  }
}
