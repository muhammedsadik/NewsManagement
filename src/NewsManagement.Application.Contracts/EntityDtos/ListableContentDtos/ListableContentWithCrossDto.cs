using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.EntityDtos.ListableContentDtos
{
  public class ListableContentWithCrossDto : ListableContentDto
  {
    public List<int> TagIds { get; set; }
    public List<int> CityIds { get; set; }
    public List<int> CategoryIds { get; set; }
    public List<int> RelatedListableContentIds { get; set; }
  }
}
