using FluentValidation;
using NewsManagement.EntityDtos.GalleryDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.Validations.GalleryValidation
{
  public class GalleryImageDtoValidator : AbstractValidator<GalleryImageDto>
  {
    public GalleryImageDtoValidator()
    {
      RuleFor(g => g.ImageId).NotEmpty();
      RuleFor(g => g.Order).NotEmpty();
      RuleFor(g => g.NewsContent).NotEmpty();

    }
  }
}
