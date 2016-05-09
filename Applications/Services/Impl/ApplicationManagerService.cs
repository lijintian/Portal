using System;
using System.Collections.Generic;
using System.Linq;
using Portal.Dto;
using DtoApplication = Portal.Dto.Application;
using DomainApplication = Portal.Domain.Aggregates.ApplictionAgg.Application;
using Portal.Domain.Repositories;
using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Applications.Services.Impl
{
    /// <summary>
    /// 表示应用系统管理服务实现
    /// </summary>
    public class ApplicationManagerService : ServiceBase, IApplicationManagerService
    {
        private readonly IApplicationRepository _appRepository;

        public ApplicationManagerService(IRepositoryContext context, IApplicationRepository appRepository)
            : base(context)
        {
            this._appRepository = appRepository;
        }

        public DtoApplication GetById(string appId)
        {
            if (string.IsNullOrEmpty(appId))
            {
                return null;
            }
            return this.ConvertToDto(this._appRepository.GetByKey(appId));
        }

        public IEnumerable<DtoApplication> GetAll()
        {
            return Array.ConvertAll(this._appRepository.GetList().OrderBy(u => u.Name).ToArray(), item => this.ConvertToDto(item));
        }

        public DtoApplication Save(DtoApplication app, SysLoggerDto logger)
        {
            Check.Argument.IsNotNull(app, "app");
            logger.IsCreate = app.IsNew();
            if (logger.IsCreate)
            {
                var newApp = new DomainApplication(app.CnName, app.EnName)
                {
                    Url = app.Url,
                    Desc = app.Desc,
                    Note = app.Remark,
                    CreatedBy = app.CreatedBy,
                    CreatedOn = app.CreatedOn,
                    UpdatedOn = app.UpdatedOn,
                    UpdatedBy = app.UpdatedBy
                };
                this._appRepository.Add(newApp);
                app.Id = newApp.Id;
            }
            else
            {
                var dbApp = this._appRepository.GetByKey(app.Id);
                dbApp.SetName(app.CnName, app.EnName);
                dbApp.SetUrl(app.Url);
                dbApp.Desc = app.Desc;
                dbApp.Note = app.Remark;
                dbApp.UpdatedOn = app.UpdatedOn;
                dbApp.UpdatedBy = app.UpdatedBy;
                this._appRepository.Update(dbApp);

            }
            Context.Commit();
            LoggerService.Create(logger, app, "{0}应用系统", "应用系统{0}成功：ID【{1}】,名称【{2}】,英文名称 【{3}】", app.Id, app.CnName, app.EnName);
            return app;
        }

        private DtoApplication ConvertToDto(DomainApplication app)
        {
            return new DtoApplication()
            {
                Id = app.Id,
                CnName = app.Name,
                EnName = app.EnName,
                Url = app.Url,
                Desc = app.Desc,
                Remark = app.Note
            };
        }
    }
}
