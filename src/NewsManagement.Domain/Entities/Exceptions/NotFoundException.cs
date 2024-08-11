using Microsoft.Extensions.Localization;
using NewsManagement.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Localization;

namespace NewsManagement.Entities.Exceptions
{
  public class NotFoundException : BusinessException, ILocalizeErrorMessage
  {
    public Type EntityType { get; set; }
    public string EntityCode { get; set; }

    public NotFoundException(Type entityType, string entityCode) : base(NewsManagementDomainErrorCodes.NotFound)
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
