/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  模块层
创建日期：  2015-11-16

内容摘要：  外部开发者信息表信息类
*/
using System;

namespace Portal.Dto
{
    [Serializable]
    public class ClientUserDto : DomainDto
    {
        #region 01.属性
        /// <summary>
        /// 用户名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsApproved { get; set; }
        /// <summary>
        /// 类型，1表示外部，2表示内部，参考枚举：ClientUserType
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
        #endregion

        #region 02.初始化
        #endregion

        #region 03.方法
        #endregion
    }

}