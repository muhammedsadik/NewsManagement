using NewsManagement.Entities.Exceptions;
using NewsManagement.Entities.Tags;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using NewsManagement.EntityDtos.TagDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using NewsManagement.EntityDtos.CityDtos;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement.Entities.Cities
{
  public class CityManager : DomainService
  {
    private readonly ICityRepository _cityRepository;
    private readonly IObjectMapper _objectMapper;

    public CityManager(ICityRepository cityRepository, IObjectMapper objectMapper)
    {
      _objectMapper = objectMapper;
      _cityRepository = cityRepository;
    }

    public async Task<CityDto> CreateAsync(CreateCityDto createCityDto)
    {
      var isExistCityName = await _cityRepository.AnyAsync(c =>
         c.CityName == createCityDto.CityName
      );
      if (isExistCityName)
        throw new AlreadyExistException(typeof(City), createCityDto.CityName.ToString());


      var isExistCityCode = await _cityRepository.AnyAsync(c =>
        c.CityCode == createCityDto.CityCode
      );
      if (isExistCityCode)
        throw new BusinessException(NewsManagementDomainErrorCodes.CityCodeAlreadyExists)
          .WithData("0", createCityDto.CityCode.ToString());

      var createCity = _objectMapper.Map<CreateCityDto, City>(createCityDto);

      var city = await _cityRepository.InsertAsync(createCity, autoSave:true);

      var cityDto = _objectMapper.Map<City, CityDto>(city);

      return cityDto;
    }

    public async Task<CityDto> UpdateAsync(int id, UpdateCityDto updateCityDto)
    {
      var existingCity = await _cityRepository.GetAsync(id);

      var isExistCityName = await _cityRepository.AnyAsync(c =>
         c.CityName == updateCityDto.CityName && c.Id != id
      );
      if (isExistCityName)
        throw new AlreadyExistException(typeof(City), updateCityDto.CityName.ToString());


      var isExistCityCode = await _cityRepository.AnyAsync(c =>
        c.CityCode == updateCityDto.CityCode && c.Id != id
      );
      if (isExistCityCode)
        throw new BusinessException(NewsManagementDomainErrorCodes.CityCodeAlreadyExists)
          .WithData("0", updateCityDto.CityCode.ToString());


      _objectMapper.Map(updateCityDto, existingCity);

      var city = await _cityRepository.UpdateAsync(existingCity, autoSave: true);

      var cityDto = _objectMapper.Map<City, CityDto>(city);

      return cityDto;
    }

    public async Task<PagedResultDto<CityDto>> GetListAsync(GetListPagedAndSortedDto input)
    {
      var totalCount = input.Filter == null
        ? await _cityRepository.CountAsync()
        : await _cityRepository.CountAsync(c => c.CityName.Contains(input.Filter));

      if (totalCount == 0)
        throw new NotFoundException(typeof(City), input.Filter ?? string.Empty);

      if (input.SkipCount >= totalCount)
        throw new BusinessException(NewsManagementDomainErrorCodes.FilterLimitsError);

      if (input.Sorting.IsNullOrWhiteSpace())
        input.Sorting = nameof(City.CityName);

      var cityList = await _cityRepository.GetListAsync(input.SkipCount, input.MaxResultCount, input.Sorting, input.Filter);

      var cityDtoList = _objectMapper.Map<List<City>, List<CityDto>>(cityList);

      return new PagedResultDto<CityDto>(totalCount, cityDtoList);
    }

    public async Task DeleteAsync(int id)
    {
      var isCityExist = await _cityRepository.AnyAsync(c => c.Id == id);
      if (!isCityExist)
        throw new EntityNotFoundException(typeof(City), id);
    }

    public async Task DeleteHardAsync(int id)
    {
      var city = await _cityRepository.GetAsync(id);

      await _cityRepository.HardDeleteAsync(city);
    }
  }
}
