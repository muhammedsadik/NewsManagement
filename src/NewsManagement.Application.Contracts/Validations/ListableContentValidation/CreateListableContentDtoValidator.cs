using FluentValidation;
using Microsoft.Extensions.Localization;
using NewsManagement.EntityDtos.ListableContentDtos;
using NewsManagement.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.Validations.ListableContentValidation
{
  public class CreateListableContentDtoValidator : AbstractValidator<CreateListableContentDto>
  {
    public CreateListableContentDtoValidator(IStringLocalizer<NewsManagementResource> localizer)
    {
      RuleFor(l => l.Title).NotEmpty();
      RuleFor(l => l.Spot).NotEmpty();
      RuleFor(l => l.listableContentType).NotEmpty().IsInEnum().WithMessage(localizer[NewsManagementDomainErrorCodes.NotInListableContentEnumType]);
    }
  }
}
