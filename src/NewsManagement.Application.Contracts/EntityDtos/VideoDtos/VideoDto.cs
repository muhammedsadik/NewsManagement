using NewsManagement.EntityConsts.VideoConsts;
using NewsManagement.EntityDtos.ListableContentDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.EntityDtos.VideoDtos
{
  public class VideoDto : ListableContentDto
  {
    public VideoType VideoType { get; set; }
  }
}
