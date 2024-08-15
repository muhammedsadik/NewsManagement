﻿using NewsManagement.Entities.ListableContentBase;
using NewsManagement.EntityConsts.VideoConsts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagement.Entities.Videos
{
  public class Video : ListableContent
  {

    public VideoType VideoType { get; set; }


    internal Video() { }

  }
}
