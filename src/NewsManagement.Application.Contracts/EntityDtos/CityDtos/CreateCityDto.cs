﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.EntityDtos.CityDtos
{
  public class CreateCityDto : EntityDto
  {
    public string CityName { get; set; }
    public int CityCode { get; set; }
  }
}
