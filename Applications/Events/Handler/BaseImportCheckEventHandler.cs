using System;
using System.Collections.Generic;
using System.Linq;
using Portal.Applications.Helper;
using Portal.Dto;
using Portal.Dto.Request;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Applications.Events.Handler
{
    public abstract class BaseImportCheckEventHandler<T, TRequest, TDto> : IDomainEventHandler<T>
        where T : BaseImportCheckEvent<TRequest>
        where TRequest : BaseImportRequest
        where TDto : class,new()
    {
        #region 抽象方法
        /// <summary>
        /// 获取Dto
        /// </summary>
        /// <returns></returns>
        public abstract TDto GetModel(TRequest item, T domainEvent, List<string> errorList);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="dto"></param>
        public abstract void Save(TDto dto, SysLoggerDto logger);
        #endregion

        #region 方法
        public virtual void Handle(T domainEvent)
        {
            throw new NotImplementedException(); //不需要实现， 因为需要CallBack
        }
        public virtual void Handle<TDomainEventResult>(T domainEvent, Action<TDomainEventResult> callback)
            where TDomainEventResult : IDomainEventResult
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");
            if (domainEvent.List == null || domainEvent.List.Count <= 0)
            {
                CheckHelper.CallBack("请上传附件！", callback);
                return;
            }
            int rowCount = domainEvent.List.Count;
            int successCount = 0;
            TDto dto = new TDto();
            foreach (var item in domainEvent.List)
            {
                List<string> errorList = new List<string>();
                try
                {
                    dto = GetModel(item, domainEvent, errorList);
                }
                catch (Exception e)
                {
                    errorList.Add("检查模板数据时出错：{0}".FormatWith(e.Message));
                }
                if (errorList.Any())
                {
                    CheckHelper.AddErrorRow(domainEvent.ErrorTable, item.RowItem, errorList);
                    continue;
                }
                try
                {
                    Save(dto, domainEvent.Logger);
                    successCount++;
                }
                catch (Exception ex)
                {
                    errorList.Add("添加数据库时出错：{0}".FormatWith(ex.Message));
                    //如果err不为空,添加到错误表
                    CheckHelper.AddErrorRow(domainEvent.ErrorTable, item.RowItem, errorList);
                }
            }
            CheckHelper.CallBack(domainEvent.ErrorTable, rowCount, successCount, callback);
        }

        protected void InitOperateInfo<T2>(T2 dto, TRequest resuest, T domainEvent) where T2 : DomainDto
        {
            dto.CreatedBy = domainEvent.Logger.CreatedBy;
            dto.CreatedOn = DateTime.Now;
        }
        #endregion
    }
}
