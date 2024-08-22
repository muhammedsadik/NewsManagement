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
  public class UpdateNewsDtoValidator : AbstractValidator<UpdateNewsDto>
  {
    public UpdateNewsDtoValidator(IStringLocalizer<NewsManagementResource> localizer)
    {
      Include(new UpdateListableContentDtoValidator(localizer));

      RuleFor(n => n.DetailImageId).NotNull();
    }
  }
}
