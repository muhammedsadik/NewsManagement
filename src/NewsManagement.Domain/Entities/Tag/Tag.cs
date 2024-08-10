using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsManagement.Entities.ListableContents;

namespace NewsManagement.Entities.Tag
{
  public class Tag : ListableContent
  {
    public string TagName { get; set; }
  }
}
