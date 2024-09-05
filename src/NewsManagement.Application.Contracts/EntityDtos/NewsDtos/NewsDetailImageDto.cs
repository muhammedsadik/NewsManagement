using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.EntityDtos.NewsDtos
{
  public class NewsDetailImageDto : IEntityDto
  {
    public Guid DetailImageId { get; set; }
  }
}
