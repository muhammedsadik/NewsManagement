using FluentValidation;
using NewsManagement.EntityDtos.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.Validations.CategoryValidation
{
  public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
  {
    public UpdateCategoryDtoValidator()
    {
      RuleFor(c => c.CategoryName).NotEmpty();
      RuleFor(c => c.ColorCode).NotEmpty();
    }
  }
}
