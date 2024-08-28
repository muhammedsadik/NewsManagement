﻿using NewsManagement.EntityConsts.CategoryConsts;
using NewsManagement.EntityConsts.ListableContentConsts;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.EntityDtos.CategoryDtos
{
  public class CategoryDto : FullAuditedEntityDto<int>
  {
    public string CategoryName { get; set; }
    public string ColorCode { get; set; }
    public bool IsActive { get; set; }
    public int? ParentCategoryId { get; set; }
    public ListableContentType listableContentType { get; set; }

  }
}
