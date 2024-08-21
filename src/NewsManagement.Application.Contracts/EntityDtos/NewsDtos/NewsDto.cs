using NewsManagement.EntityDtos.ListableContentDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.EntityDtos.NewsDtos
{
  public class NewsDto : ListableContentDto
  {
    public List<Guid>? DetailImageId { get; set; }
  }
}
