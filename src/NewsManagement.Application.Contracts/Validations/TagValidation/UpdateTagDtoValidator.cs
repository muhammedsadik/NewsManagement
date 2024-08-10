using FluentValidation;
using Microsoft.Extensions.Localization;
using NewsManagement.EntityDtos.TagDtos;
using NewsManagement.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.Validations.TagValidation
{
  public class UpdateTagDtoValidator : AbstractValidator<UpdateTagDto>
  {
    public UpdateTagDtoValidator(IStringLocalizer<NewsManagementResource> localizer)
    {
      RuleFor(t => t.TagName).NotEmpty();
    }

  }
}
