using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.EntityDtos.ListableContentsDto
{
  public class UpdateListableContentDto : EntityDto
  {
    public string Title { get; set; }
    public string Spot { get; set; }
    public bool Status { get; set; }
  }
}
