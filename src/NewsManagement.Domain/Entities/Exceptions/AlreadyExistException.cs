using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Localization;
using Volo.Abp;
using NewsManagement.Localization;
using Microsoft.Extensions.Localization;

namespace NewsManagement.Entities.Exceptions
{
  public class AlreadyExistException : BusinessException, ILocalizeErrorMessage
  {
    public Type EntityType { get; }
    public string EntityCode { get; }

    public AlreadyExistException(Type entityType, string entityCode) : base(NewsManagementDomainErrorCodes.AlreadyExists)
    {
      EntityType = entityType;
      EntityCode = entityCode;
    }

    public string LocalizeMessage(LocalizationContext context)
    {
      var localizer = context.LocalizerFactory.Create<NewsManagementResource>();

      Data["EntityType"] = localizer[EntityType.Name!].Value;
      Data["EntityCode"] = EntityCode;

      return localizer[Code!, Data["EntityType"], Data["EntityCode"]];
    }
  }
}
