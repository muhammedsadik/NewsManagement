﻿using NewsManagement.Entities.ListableContentBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagement.Entities.Newses
{
  public class News : ListableContent
  {
    public byte[] DetailPicture { get; set; }

    internal News() { }
  }
}
