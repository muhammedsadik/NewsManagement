﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.EntityDtos.TagDtos
{
  public class CreateTagDto : EntityDto
  {
    public string TagName { get; set; }

  }
}
