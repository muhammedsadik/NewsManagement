using NewsManagement.EntityDtos.ListableContentsDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.EntityDtos.TagDtos
{
  public class UpdateTagDto : UpdateListableContentDto
  {
    public string TagName { get; set; }
  }
}
