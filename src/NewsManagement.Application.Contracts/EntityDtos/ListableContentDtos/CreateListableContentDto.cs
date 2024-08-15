using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.CategoryDtos;
using NewsManagement.EntityDtos.CityDtos;
using NewsManagement.EntityDtos.TagDtos;
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
    public List<CreateCategoryDto> CreateCategoryDtos { get; set; }
    public List<CreateCityDto> CreateCityDtos { get; set; }
    public List<CreateTagDto> CreateTagDtos { get; set; }
  }
}
