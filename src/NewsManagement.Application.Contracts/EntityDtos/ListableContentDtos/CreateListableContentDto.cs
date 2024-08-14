using NewsManagement.EntityConsts.ListableContentConsts;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.EntityDtos.ListableContentDtos
{
  public class CreateListableContentDto : EntityDto
  {
    public string Title { get; set; }
    public string Spot { get; set; }
    public bool Status { get; set; }
    public int ImageId { get; set; }
    public ListableContentType listableContentType { get; set; }
  }
}
