using FluentValidation;
using Microsoft.Extensions.Localization;
using NewsManagement.EntityDtos.VideoDtos;
using NewsManagement.Localization;
using NewsManagement.Validations.ListableContentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.Validations.VideoValidation
{
  public class CreateVideoDtoValidator : AbstractValidator<CreateVideoDto>
  {
    public CreateVideoDtoValidator(IStringLocalizer<NewsManagementResource> localizer)
    {
      Include(new CreateListableContentDtoValidator(localizer));

      RuleFor(v => v.VideoType).NotEmpty().IsInEnum().WithMessage(localizer[NewsManagementDomainErrorCodes.NotInVideoEnumType]);
    }
  }
}
