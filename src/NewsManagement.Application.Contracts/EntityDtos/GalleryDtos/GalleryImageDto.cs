using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.EntityDtos.GalleryDtos
{
  public class GalleryImageDto : IEntityDto
  {
    public Guid ImageId { get; set; }// ❓ ImageId ye ait bir item varmı kontrolünü yap
    public string NewsContent { get; set; }
  }
}
