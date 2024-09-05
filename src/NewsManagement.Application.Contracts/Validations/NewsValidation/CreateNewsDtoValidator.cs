using FluentValidation;
using Microsoft.Extensions.Localization;
using NewsManagement.EntityDtos.NewsDtos;
using NewsManagement.Localization;
using NewsManagement.Validations.GalleryValidation;
using NewsManagement.Validations.ListableContentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.Validations.NewsValidation
{
  public class CreateNewsDtoValidator : AbstractValidator<CreateNewsDto>
  {
    public CreateNewsDtoValidator(IStringLocalizer<NewsManagementResource> localizer)
    {
      Include(new CreateListableContentDtoValidator(localizer));

      RuleFor(x => x.DetailImageIds).ForEach(x => x.SetValidator(new NewsDetailImageDtoValidator()));
    }
  }
}
