﻿using NewsManagement.EntityConsts.VideoConsts;
using NewsManagement.EntityDtos.ListableContentDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.EntityDtos.VideoDtos
{
  public class CreateVideoDto : CreateListableContentDto
  {
    public VideoType VideoType { get; set; }// bunun hakkında bilgi sahibi ol.
    public string? Url { get; set; }
  }
}
