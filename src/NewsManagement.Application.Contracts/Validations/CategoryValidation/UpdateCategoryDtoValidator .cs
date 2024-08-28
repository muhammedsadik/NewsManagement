using FluentValidation;
using Microsoft.Extensions.Localization;
using NewsManagement.EntityDtos.CategoryDtos;
using NewsManagement.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.Validations.CategoryValidation
{
  public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
  {
    public UpdateCategoryDtoValidator(IStringLocalizer<NewsManagementResource> localizer)
    {
      RuleFor(c => c.CategoryName).NotEmpty();
      RuleFor(c => c.ColorCode).NotEmpty();

      RuleFor(c => c.listableContentType).IsInEnum().WithMessage(localizer[NewsManagementDomainErrorCodes.NotInListableContentEnumType]);

    }
  }
}
