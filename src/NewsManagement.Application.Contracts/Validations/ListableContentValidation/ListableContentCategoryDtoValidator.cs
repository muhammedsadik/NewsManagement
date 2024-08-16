using FluentValidation;
using NewsManagement.EntityDtos.ListableContentDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.Validations.ListableContentValidation
{
  public class ListableContentCategoryDtoValidator : AbstractValidator<ListableContentCategoryDto>
  {
    public ListableContentCategoryDtoValidator()
    {
      RuleFor(x => x.CategoryId).NotEmpty();

    }
  }
}
