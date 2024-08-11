using FluentValidation;
using NewsManagement.EntityDtos.CityDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.Validations.CityValidation
{
  public class CreateCityDtoValidator : AbstractValidator<CreateCityDto>
  {
    public CreateCityDtoValidator()
    {
      RuleFor(c => c.CityName).NotEmpty();
      RuleFor(c => c.CityCode).NotEmpty();
    }

  }
}
