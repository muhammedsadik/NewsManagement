using FluentValidation;
using Microsoft.Extensions.Localization;
using NewsManagement.EntityDtos.GalleryDtos;
using NewsManagement.Localization;
using NewsManagement.Validations.ListableContentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.Validations.GalleryValidation
{
  public class UpdateGalleryDtoValidator : AbstractValidator<UpdateGalleryDto>
  {
    public UpdateGalleryDtoValidator(IStringLocalizer<NewsManagementResource> localizer)
    {
      Include(new UpdateListableContentDtoValidator(localizer));

      RuleFor(x => x.GalleryImages).ForEach(x => x.SetValidator(new GalleryImageDtoValidator()));

    }
  }
}
