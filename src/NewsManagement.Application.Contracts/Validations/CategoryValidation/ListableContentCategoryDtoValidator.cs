using FluentValidation;
using NewsManagement.EntityDtos.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.Validations.CategoryValidation
{
  public class ListableContentCategoryDtoValidator : AbstractValidator<ListableContentCategoryDto>
  {
    public ListableContentCategoryDtoValidator()
    {
      RuleFor(x => x.CategoryId).NotEmpty();

    }
  }
}
