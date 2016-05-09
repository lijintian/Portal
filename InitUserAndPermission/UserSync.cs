using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ToolBLL.portal;
using ToolBLL.yewu;

namespace InitUserAndPermission
{
    public class UserSync
    {
        string[] _CustomerRole = ConfigurationManager.AppSettings["CustomerRole"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        string[] _EmployeeRole = ConfigurationManager.AppSettings["EmployeeRole"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        private PortalUserManage portalUserManage = new ToolBLL.portal.PortalUserManage();
        private PortalPermissionManage portalPermissionManage = new PortalPermissionManage();
        private List<ToolBLL.portal.Permission> portalPermissions;
        private List<ClientUserAuth> clientUserAuths;

        public UserSync()
        {
            portalPermissions = portalPermissionManage.GetPermission();
            //获取业务系统所有客户对应的权限信息
            clientUserAuths = new UserManage().GetClientUserAuth();
        }

        public void InitUser()
        {
            //1：获取业务系统的所有用户信息
            var yewuUsers = new UserManage().GetUser2();
            int index = 1;
            foreach (var item in yewuUsers)
            {
                if (string.IsNullOrEmpty(item.LoginName))
                {
                    Console.WriteLine("业务系统用户添加失败，失败原因：用户名不能为空");
                    continue;
                }
                var portalUser = portalUserManage.GetUserByLoginName(item.LoginName);
                if (portalUser == null || string.IsNullOrEmpty(portalUser._id))
                {
                    Console.WriteLine(string.Format("{0}、新增用户【{1}】", index, item.LoginName));
                    Create(item);
                }
                else
                {
                    Console.WriteLine(string.Format("{0}、修改用户【{1}】", index, item.LoginName));
                    Update(item);
                }
                index++;
            }
        }

        public void Create(ToolBLL.yewu.User item)
        {
            var permissions = new List<string>();
            if (item.ClientID > 0)
            {
                var clientUserAuth = clientUserAuths.FirstOrDefault(s => s.UserID == item.UserID);
                if (clientUserAuth != null)
                {
                    permissions = clientUserAuth.Auths.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }
            else
            {
                permissions = item.Purview == null ? new List<string>() : item.Purview.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            List<string> newPermissions = new List<string>();
            foreach (var permission in permissions)
            {
                if (portalPermissions.Find(s => s.Code == permission) == null)
                    continue;
                if (!newPermissions.Contains(permission))
                    newPermissions.Add(permission);
            }
            List<string> roles = new List<string>();
            if (item.ClientID > 0)
            {
                roles.AddRange(_CustomerRole);
            }
            else
            {
                roles.AddRange(_EmployeeRole);
            }
            var newportalUser = new ToolBLL.portal.User()
            {
                LoginName = item.LoginName,
                Password = item.Password,
                DisplayName = item.DisplayName,
                Email = item.Email,
                Desc = item.Desc,
                EmployeeNo = item.EmployeeNo,
                ClientNo = item.ClientNo,
                IsApproved = item.IsApproved,
                CreatedOn = item.CreatedOn,
                LastLoginedIp = item.LastLoginedIp,
                LastLoginedTime = item.LastLoginedTime,
                UserType = item.ClientID > 0 ? UserType.Customer : UserType.Employee,
                Permissions = newPermissions,
                Roles = roles,
                _t = "User"
            };
            //2：更新到portal
            portalUserManage.InsertUser(newportalUser);
        }

        public void Update(ToolBLL.yewu.User item)
        {
            var permissions = new List<string>();
            if (item.ClientID > 0)
            {
                var clientUserAuth = clientUserAuths.FirstOrDefault(s => s.UserID == item.UserID);
                if (clientUserAuth != null)
                {
                    permissions = clientUserAuth.Auths.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }
            else
            {
                permissions = item.Purview == null ? new List<string>() : item.Purview.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            List<string> newPermissions = new List<string>();
            foreach (var permission in permissions)
            {
                if (portalPermissions.Find(s => s.Code == permission) == null)
                    continue;
                if (!newPermissions.Contains(permission))
                    newPermissions.Add(permission);
            }
            List<string> roles = new List<string>();
            if (item.ClientID > 0)
            {
                roles.AddRange(_CustomerRole);
            }
            else
            {
                roles.AddRange(_EmployeeRole);
            }
            var newportalUser = new ToolBLL.portal.User()
            {
                LoginName = item.LoginName,
                Password = item.Password,
                DisplayName = item.DisplayName,
                Email = item.Email,
                Desc = item.Desc,
                EmployeeNo = item.EmployeeNo,
                IsApproved = item.IsApproved,
                CreatedOn = item.CreatedOn,
                LastLoginedIp = item.LastLoginedIp,
                LastLoginedTime = item.LastLoginedTime,
                UserType = item.ClientID > 0 ? UserType.Customer : UserType.Employee,
                Roles = roles,
                Permissions = newPermissions,
                _t = "User"
            };
            portalUserManage.UpdateUser(newportalUser);
        }
    }
}
