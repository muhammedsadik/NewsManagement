﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.EntityDtos.CategoryDtos
{
  public class UpdateCategoryDto : EntityDto
  {
    public bool IsActive { get; set; }
    public string CategoryName { get; set; }
    public string ColorCode { get; set; }
    public int? ParentCategoryId { get; set; }
  }
}
