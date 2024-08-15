using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NewsManagement.EntityDtos.GalleryDtos
{
  public interface IGalleryAppService : ICrudAppService<GalleryDto, int, GetListPagedAndSortedDto, CreateGalleryDto, UpdateGalleryDto>
  {
    Task DeleteHardAsync(int id);
  }
}
