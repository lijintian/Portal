/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  应用层
创建日期：  2016-03-02

内容摘要：  表管理服务类
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Portal.Domain.Aggregates;
using Portal.Domain.Repositories;
using Portal.Domain.Specification;
using Portal.Dto;
using Portal.Web.Core;
using SortOrder = EasyDDD.Infrastructure.Crosscutting.Paged.SortOrder;
using SysLoggerType = Portal.Dto.SysLoggerType;
using EasyDDD.Core.Specification;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;

namespace Portal.Applications.Services.Impl
{
    public class SysLoggerManagerService : PortalService<SysLoggerDto, FindSysLoggerRequest, SysLogger>, ISysLoggerManagerService
    {
        #region 01.初始化
        public SysLoggerManagerService(IRepositoryContext context, ISysLoggerRepository repository)
            : base(context, repository)
        {
        }
        #endregion

        #region 02.属性
        private IApplicationRepository _applicationRepository { get { return IoC.Resolve<IApplicationRepository>(); } }
        #endregion

        #region 获取
        public FindSysLoggerResponse Search(FindSysLoggerRequest request)
        {
            Check.Argument.IsNotNull(request, "request");
            var order = new Dictionary<Expression<Func<SysLogger, object>>, SortOrder>();
            order.Add(item => item.CreatedOn, SortOrder.Descending);
            return Search<FindSysLoggerResponse>(request, order);
        }
        #endregion

        #region 03.操作
        public void Save(SysLoggerDto request)
        {
            if (request.IsNew())
            {
                Create(request);
            }
            else
            {
                Update(request);
            }
            this.Context.Commit();
        }

        public void Create<T>(SysLoggerDto logger, T dto, string title, string content, params object[] args) where T : AggregateDto
        {
            if (logger == null) return;
            SysLoggerType type = logger.IsCreate ? SysLoggerType.Create : SysLoggerType.Update;
            Create(logger, title, FormatWith2(content, type.GetDescription(), args), type);
        }

        public void Create(SysLoggerType type, SysLoggerDto logger, string title, string content, params object[] args)
        {
            if (logger == null) return;
            Create(logger, title, FormatWith2(content, type.GetDescription(), args), type);
        }
        #endregion

        #region 04.添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">实体</param>
        /// <returns></returns>
        private void Create(SysLoggerDto info)
        {
            //Server.UrlDecode(info.Url);
            var domain = new SysLogger(info.ApplicationName, info.Ip, info.Url, info.Title, info.Content, info.CreatedBy);
            this.Map(info, domain);
            base.Create(domain);
            info.Id = domain.Id;
        }
        #endregion

        #region 05.更新
        /// <summary>
        /// 更新
        /// </summary>
        private void Update(SysLoggerDto info)
        {
            var domain = GetDomain(info.Id);
            this.Map(info, domain);
            domain.InitUpdateInfo(info.UpdatedBy);
            base.Update(domain);
        }
        #endregion

        #region 06.删除
        public void Delete(string id)
        {
            Check.Argument.IsNotNull(id, "id");
            var domain = this.GetDomain(id);
            //domain.InitUpdateInfo(updateBy);
            base.Delete(domain);
            this.Context.Commit();
        }

        public void DeleteMore(string ids)
        {
            Check.Argument.IsNotNull(ids, "ids");
            var domainList = this.GetDomainList(new SysLoggerIdListSpecification(ids));
            foreach (var item in domainList)
            {
                base.Delete(item);
            }
            this.Context.Commit();
        }
        #endregion

        #region 07.抽象方法实现
        protected override ISpecification<SysLogger> ConvertToSpec(FindSysLoggerRequest request)
        {
            var specs = new List<ISpecification<SysLogger>>();
            if (!string.IsNullOrEmpty(request.Id))
            {
                specs.Add(new SysLoggerIdSpecification(request.Id));
            }
            if (!string.IsNullOrEmpty(request.ApplicationName))
            {
                specs.Add(new SysLoggerApplicationNameSpecification(request.ApplicationName));
            }
            if (!string.IsNullOrEmpty(request.CreatedBy))
            {
                specs.Add(new SysLoggerContainCreatedBySpecification(request.CreatedBy));
            }
            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                specs.Add(new SysLoggerContainKeyWordSpecification(request.KeyWord));
            }
            if (request.Level.HasValue)
            {
                specs.Add(new SysLoggerLevelSpecification(SysLoggerTypeMapper.MapToLevel(request.Level.Value)));
            }
            if (request.Type.HasValue)
            {
                specs.Add(new SysLoggerTypeSpecification(SysLoggerTypeMapper.MapToType(request.Type.Value)));
            }
            if (request.Right.HasValue)
            {
                specs.Add(new SysLoggerRightSpecification(SysLoggerTypeMapper.MapToRight(request.Right.Value)));
            }
            if (request.StartTime.HasValue)
            {
                specs.Add(new SysLoggerCreatedOnStartSpecification(request.StartTime.Value));
            }
            if (request.EndTime.HasValue)
            {
                specs.Add(new SysLoggerCreatedOnEndSpecification(request.EndTime.Value));
            }
            return GetSpecs(specs);
        }

        protected override SysLoggerDto Map(SysLogger source)
        {
            return new SysLoggerDto()
            {
                Id = source.Id,
                ApplicationName = source.ApplicationName,
                Title = source.Title,
                Content = source.Content,
                Ip = source.Ip,
                Url = source.Url,
                Browser = source.Browser,
                OS = source.OS,
                Level = SysLoggerTypeMapper.MapToLevel(source.Level),
                Type = SysLoggerTypeMapper.MapToType(source.Type),
                Right = SysLoggerTypeMapper.MapToRight(source.Right),
                CreatedBy = source.CreatedBy,
                CreatedOn = source.CreatedOn
            };
        }
        #endregion

        #region 08.私有方法
        //private void SetApplicationName(IEnumerable<SysLoggerDto> appPers)
        //{
        //    var apps = this._applicationRepository.GetList();
        //    foreach (var per in appPers)
        //    {
        //        if (string.IsNullOrEmpty(per.ApplicationId)) continue;
        //        per.ApplicationName = apps.First(item => item.Id == per.ApplicationId).Name;
        //    }
        //}
        private void Map(SysLoggerDto dto, SysLogger domain)
        {
            domain.Browser = dto.Browser;
            domain.OS = dto.OS;
            domain.Level = SysLoggerTypeMapper.MapToLevel(dto.Level);
            domain.Type = SysLoggerTypeMapper.MapToType(dto.Type);
            domain.Right = SysLoggerTypeMapper.MapToRight(dto.Right);
        }
        private void Create(SysLoggerDto logger, string title, string content, SysLoggerType type, Dto.SysLoggerLevel level = Dto.SysLoggerLevel.Info, Dto.SysLoggerRight right = Dto.SysLoggerRight.All)
        {
            try
            {
                string typeName = type.GetDescription();
                logger.InitInfo(FormatWith2(title, typeName), FormatWith2(content, typeName), type, level, right);
                Create(logger, true);
            }
            catch
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="isThread">是否启用线程</param>
        private void Create(SysLoggerDto dto, bool isThread)
        {
            if (isThread)
            {
                //发起线程来做，节省时间
                ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                {
                    this.Save(dto);
                }));
            }
            else
            {
                this.Save(dto);
            }
        }
        /// <summary>
        /// Formats a string. Similar to String.Format()
        /// </summary>
        /// <param name="target">string to format</param>
        /// <param name="args">Arguments to replace in the string.</param>
        /// <returns>Formatted string.</returns>
        private string FormatWith2(string target, string str, params  object[] args)
        {
            if (string.IsNullOrEmpty(target)) return target;
            if (args == null)
            {
                return string.Format(target, str);
            }
            List<string> strList = args.Select(u => u == null ? string.Empty : u.ToString()).ToList();
            strList.Insert(0, str);
            return String.Format(target, strList.ToArray());
        }
        #endregion
    }
}
