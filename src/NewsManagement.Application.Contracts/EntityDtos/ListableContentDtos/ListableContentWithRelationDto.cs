using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.EntityDtos.ListableContentDtos
{
  public class ListableContentWithRelationDto : ListableContentDto
  {
    public List<ListableContentDto> ListableContents { get; set; }
  }
}
