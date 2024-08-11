using FluentValidation;
using NewsManagement.EntityDtos.CityDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.Validations.CityValidation
{
  public class UpdateCityDtoValidator : AbstractValidator<UpdateCityDto>
  {
    public UpdateCityDtoValidator()
    {
      RuleFor(c => c.CityName).NotEmpty();
      RuleFor(c => c.CityCode).NotEmpty();
    }
  }
}
