using FluentValidation;
using Microsoft.Extensions.Localization;
using NewsManagement.EntityDtos.NewsDtos;
using NewsManagement.Localization;
using NewsManagement.Validations.ListableContentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.Validations.NewsValidation
{
  public class NewsDetailImageDtoValidator : AbstractValidator<NewsDetailImageDto>
  {
    public NewsDetailImageDtoValidator()
    {
      RuleFor(n => n.DetailImageId).NotNull();
    }
  }
}
