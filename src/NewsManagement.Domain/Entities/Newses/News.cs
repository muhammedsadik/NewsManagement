﻿using NewsManagement.Entities.ListableContents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagement.Entities.Newses
{
  public class News : ListableContent
  {
    public Guid[]? DetailImageId { get; set; }

    internal News() { }
  }
}
