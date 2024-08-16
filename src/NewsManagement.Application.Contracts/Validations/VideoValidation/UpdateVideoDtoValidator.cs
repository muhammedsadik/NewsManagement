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
  public class UpdateVideoDtoValidator : AbstractValidator<UpdateVideoDto>
  {
    public UpdateVideoDtoValidator(IStringLocalizer<NewsManagementResource> localizer)
    {
      Include(new UpdateListableContentDtoValidator(localizer)); 
      
      RuleFor(v => v.VideoType).NotEmpty().IsInEnum().WithMessage(localizer[NewsManagementDomainErrorCodes.NotInVideoEnumType]);
    }
  }
}
