using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.EntityDtos.CategoryDtos
{
  public class CategoryDto : FullAuditedEntityDto<int>
  {
    public bool IsActive { get; set; }
    public string CategoryName { get; set; }
    public string ColorCode { get; set; }
  }
}
