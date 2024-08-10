using NewsManagement.EntityDtos.ListableContentsDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.EntityDtos.TagDtos
{
  public class TagDto : ListableConentDto
  {
    public string TagName { get; set; }
  }
}
