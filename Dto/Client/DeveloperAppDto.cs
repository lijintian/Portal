using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Dto
{
    /// <summary>
    /// 表示开发者应用传输对象
    /// </summary>
    public class DeveloperAppDto : AggregateDto
    {
        public DeveloperAppDto()
        {
            this.RequestGroupList = new List<string>();
            this.RequestPermssions = new List<string>();
            this.ApprovedGroupList = new List<string>();
            this.ApprovedPermssions = new List<string>();
        }

        #region 属性
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc
        {
            get;
            set;
        }

        /// <summary>
        /// 应用访问地址
        /// </summary>
        public string AccessUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 应用回调地址
        /// </summary>
        public string CallbackUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 应用类型
        /// </summary>
        public DeveloperAppType AppType
        {
            get;
            set;
        }

        /// <summary>
        /// 应用状态
        /// </summary>
        public DeveloperAppState State
        {
            get;
            set;
        }

        /// <summary>
        /// 标识是否外部App
        /// </summary>
        public bool IsExternal
        {
            get;
            set;
        }

        /// <summary>
        /// 应用所属开发者账号Id
        /// </summary>
        public string UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks
        {
            get;
            set;
        }


        /// <summary>
        /// 应用标识Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 应用密码
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 请求审核API权限分组列表
        /// </summary>
        public List<string> RequestGroupList { get; set; }

        /// <summary>
        /// 请求审核权限
        /// </summary>
        public List<string> RequestPermssions
        {
            get;
            set;
        }

        /// <summary>
        /// 审核通过的API权限分组列表
        /// </summary>
        public List<string> ApprovedGroupList { get; set; }

        /// <summary>
        /// 已审权限
        /// </summary>
        public List<string> ApprovedPermssions
        {
            get;
            set;
        }

        /// <summary>
        /// 请求审核分组信息
        /// </summary>
        public string RequestGroupDesc { get; set; }

        /// <summary>
        /// 已审分组信息
        /// </summary>
        public string ApprovedGroupDesc { get; set; }

        /// <summary>
        /// 请求审核权限信息
        /// </summary>
        public string RequestPermssionDesc { get; set; }

        /// <summary>
        /// 已审权限信息
        /// </summary>
        public string ApprovedPermssionDesc { get; set; }
        #endregion

        #region 方法
        public bool ExistsRequestGroup()
        {
            return this.RequestGroupList != null && this.RequestGroupList.Any();
        }
        public bool ExistsApprovedGroup()
        {
            return this.ApprovedGroupList != null && this.ApprovedGroupList.Any();
        }
        public bool ExistsRequestPermssion()
        {
            return this.RequestPermssions != null && this.RequestPermssions.Any();
        }
        public bool ExistsApprovedPermssion()
        {
            return this.ApprovedPermssions != null && this.ApprovedPermssions.Any();
        }
        #endregion
    }
}
