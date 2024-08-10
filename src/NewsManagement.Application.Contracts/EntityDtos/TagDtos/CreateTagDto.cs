using NewsManagement.EntityDtos.ListableContentsDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.EntityDtos.TagDtos
{
  public class CreateTagDto : CreateListableContentDto
  {
    public string TagName { get; set; }

  }
}
