using NewsManagement.EntityDtos.ListableContentDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.EntityDtos.NewsDtos
{
  public class CreateNewsDto : CreateListableContentDto
  {
    public Guid[]? DetailImageId { get; set; }
  }
}
