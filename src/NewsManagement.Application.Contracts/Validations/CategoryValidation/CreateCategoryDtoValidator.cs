using FluentValidation;
using NewsManagement.EntityDtos.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.Validations.CategoryValidation
{
  public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
  {
    public CreateCategoryDtoValidator()
    {
      RuleFor(c => c.CategoryName).NotEmpty();
      RuleFor(c => c.ColorCode).NotEmpty();
    }
  }
}
