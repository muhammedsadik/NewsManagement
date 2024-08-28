using FluentValidation;
using Microsoft.Extensions.Localization;
using NewsManagement.EntityDtos.CategoryDtos;
using NewsManagement.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.Validations.CategoryValidation
{
  public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
  {
    public CreateCategoryDtoValidator(IStringLocalizer<NewsManagementResource> localizer)
    {
      RuleFor(c => c.CategoryName).NotEmpty();
      RuleFor(c => c.ColorCode).NotEmpty();

      RuleFor(c => c.listableContentType).IsInEnum().WithMessage(localizer[NewsManagementDomainErrorCodes.NotInListableContentEnumType]);

    }
  }
}
