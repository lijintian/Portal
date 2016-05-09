using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBLL.portal
{
    public class Permission
    {
        public Permission()
        {
            this._id = ObjectId.GenerateNewId().ToString();
        }
        public string _id { get; set; }

        public string _t { get; set; }

        /// <summary>
        /// 所属应用程序
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 权限附属标志
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否兼容老的业务系统
        /// </summary>
        public bool IsCompatible { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Desc { get; set; }

        public string ParentId { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public List<string> FunctionPermissions { get; set; }
        public byte[] RowVersion { get; set; }

    }
}
