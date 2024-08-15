using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.EntityDtos.CategoryDtos
{
  public class ListableContentCategoryDto : IEntityDto
  {
    public int CategoryId { get; set; }
    public bool IsPrimary { get; set; }
  }
}
