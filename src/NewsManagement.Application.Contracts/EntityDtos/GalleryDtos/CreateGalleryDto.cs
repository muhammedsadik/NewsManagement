using NewsManagement.EntityDtos.ListableContentDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.EntityDtos.GalleryDtos
{
  public class CreateGalleryDto : CreateListableContentDto
  {
    public List<GalleryImageDto> GalleryImages { get; set; }
  }
}
