using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolBLL.portal;
using ToolBLL.yewu;
using ToolBLL;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace SyncModifiedPortalData
{
    class Program
    {
        public static ILog log = LogManager.GetLogger("Program");
        static void Main(string[] args)
        {
            Console.WriteLine("Start...");
            try
            {
                MongoDBHelper.InitMongodb();
                var infos = new PortalInfoManage().GetPortalInfo();
                foreach (var item in infos)
                {
                    try
                    {
                        if (item.Type == PortalInfoType.UserInfo)
                        {
                            UpdateUserInfoByProtal(item.ModifiedId);
                        }
                        else if (item.Type == PortalInfoType.UserPermission)
                        {
                            UpdateUserPermissionByProtal(item.ModifiedId);
                        }
                        //else if (item.Type == PortalInfoType.Permission)
                        //{
                        //    InsertPermission(item.ModifiedId);
                        //}
                    }
                    catch (Exception ex)
                    {
                        log.Error("同步更新失败,原因：" + ex.Message);
                        log.Info("ModifiedId：" + item.ModifiedId + "；Type：" + item.Type + "；UpdateOn：" + item.UpdateOn);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("同步更新失败,原因：" + ex.Message);
            }
            Console.WriteLine("End");
        }
        /// <summary>
        /// 从portal系统 同步更新用户信息到 业务系统
        /// </summary>
        static void UpdateUserInfoByProtal(string modifiedId)
        {
            UserManage userManage = new UserManage();
            PortalUserManage portalUserManage = new ToolBLL.portal.PortalUserManage();
            ToolBLL.portal.User portalUser = portalUserManage.GetUserByID(modifiedId);
            if (portalUser != null)
            {
                ToolBLL.yewu.User user = userManage.GetUserByLoginName(portalUser.LoginName);
                if (user != null)
                {
                    user.DisplayName = portalUser.DisplayName;
                    user.Email = portalUser.Email;
                    user.Desc = portalUser.Desc;
                    user.IsApproved = portalUser.IsApproved;
                    user.Password = portalUser.Password;
                    //更新登陆账号信息
                    userManage.UpdateUser(user);
                    if (portalUser.UserType == UserType.Customer)
                    {
                        //更新客户信息
                        userManage.UpdateClient(user);
                    }
                }
            }
        }

        /// <summary>
        /// 从portal系统 同步更新用户对应的权限到 业务系统
        /// </summary>
        static void UpdateUserPermissionByProtal(string modifiedId)
        {
            UserManage userManage = new UserManage();
            PermissionManage permissionManage = new PermissionManage();
            PortalUserManage portalUserManage = new ToolBLL.portal.PortalUserManage();
            ToolBLL.portal.User portalUser = portalUserManage.GetUserByID(modifiedId);
            if (portalUser != null)
            {
                ToolBLL.yewu.User user = userManage.GetUserByLoginName(portalUser.LoginName);
                if (user != null)
                {
                    //页面权限
                    var permissions = "," + string.Join(",", portalUser.Permissions.ToArray()) + ",";
                    var rolePermissions = string.Empty;
                    if (portalUser.Roles != null && portalUser.Roles.Count > 0)//存在角色
                    {
                        foreach (var pcode in portalUser.Roles)
                        {
                            var roleInfo = portalUserManage.GetRoleByCode(pcode);
                            if (roleInfo != null && roleInfo.Permissions != null)
                            {
                                roleInfo.Permissions.ForEach((item) =>
                                {
                                    if (!permissions.Contains("," + item + ","))
                                    {
                                        //角色权限
                                        permissions += item + ",";
                                    }
                                });
                            }
                        }
                    }

                    if (user.ClientID > 0)
                    {
                        var info = new ClientUserAuth()
                        {
                            UserID = user.UserID,
                            Auths = permissions
                        };
                        //更新客户的权限信息
                        if (userManage.HasUserAuthByUserID(info.UserID))
                        {
                            userManage.UpdateClientUserAuth(info);
                        }
                        else
                        {
                            userManage.InsertClientUserAuth(info);
                        }
                    }
                    else
                    { 
                        user.Purview = permissions;
                        //更新登陆用户的权限信息
                        userManage.UpdateUserPermission(user);
                    }
                }
            }
        }

        /// <summary>
        ///  从portal系统 同步更新到 业务系统
        /// </summary>
        static void InsertPermission(string modifiedId)
        {
            PermissionManage permissionManage = new PermissionManage();
            PortalPermissionManage portalPermissionManage = new PortalPermissionManage();
            //获取portal权限信息
            var portalPermission = portalPermissionManage.GetPermissionByID(modifiedId);
            if (portalPermission != null)
            {
                string classKey = "," + portalPermission.Code + ",", classRemark = portalPermission.Desc, classSign = "0";
                foreach (var code in portalPermission.FunctionPermissions)
                {
                    //获取portal权限信息
                    var portalFunctionPermission = portalPermissionManage.GetPermissionByCode(code);
                    if (portalFunctionPermission != null)
                    {
                        classRemark += "|" + portalFunctionPermission.Name;
                        classSign += portalFunctionPermission.Tag.ToString();
                    }
                }
                //新增到 业务系统
                permissionManage.InsertPermission(new ToolBLL.yewu.Permission()
                {
                    ClassName = portalPermission.Name,
                    ClassKey = classKey,
                    ClassRemark = classRemark,
                    ClassSign = classSign,
                    DealFlag = 0,
                    Deeps = 1,
                    MenuList = Convert.ToInt32(portalPermission.Tag),
                    ParentID = Convert.ToInt64(portalPermission.ParentId)
                });

            }
        }

    }
}
