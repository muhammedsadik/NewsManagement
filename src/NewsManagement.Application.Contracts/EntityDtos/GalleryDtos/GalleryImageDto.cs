using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.EntityDtos.GalleryDtos
{
  public class GalleryImageDto : IEntityDto
  {
    public byte Picture { get; set; }
    public string NewsContent { get; set; }
  }
}
