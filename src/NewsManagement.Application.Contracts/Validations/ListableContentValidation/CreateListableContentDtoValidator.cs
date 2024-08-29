using FluentValidation;
using Microsoft.Extensions.Localization;
using NewsManagement.EntityDtos.ListableContentDtos;
using NewsManagement.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewsManagement.Validations.ListableContentValidation
{
  public class CreateListableContentDtoValidator : AbstractValidator<CreateListableContentDto>
  {
    public CreateListableContentDtoValidator(IStringLocalizer<NewsManagementResource> localizer)
    {
      RuleFor(l => l.Title).NotEmpty();
      RuleFor(l => l.Spot).NotEmpty();
      RuleFor(l => l.TagIds).NotEmpty();
      RuleFor(l => l.CityIds).NotEmpty();
      RuleFor(l => l.Status).IsInEnum().WithMessage(localizer[NewsManagementDomainErrorCodes.NotInStatusEnumType]);

      RuleFor(l => l.ListableContentCategoryDtos)
        .Must(cat => cat == null || cat.Count(c => c.IsPrimary) == 1)
        .WithMessage(x => string.Format(
          localizer[NewsManagementDomainErrorCodes.OnlyOneCategoryIsActiveStatusTrue],
          x.ListableContentCategoryDtos?.Count(c => c.IsPrimary) ?? 0)
        );


      //RuleFor(x => x.ListableContentCategoryDtos).ForEach(x => x.SetValidator(new ListableContentCategoryDtoValidator()));
    }
  }
}
