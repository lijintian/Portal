using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ToolBLL.portal;
using ToolBLL.yewu;
using Permission = ToolBLL.portal.Permission;
using User = ToolBLL.portal.User;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace InitUserAndPermission
{
    class Program
    {
        public static ILog log = LogManager.GetLogger("Program");
        public static string _ApplicationId = ConfigurationManager.AppSettings["YEWU_ApplicationId"];
        public static string _ClientApplicationId = ConfigurationManager.AppSettings["YEWU_Client_ApplicationId"];
        public static string YwUrl = "";
        public static string YwClientUrl = "";
        public static string ReplaceUrl = "../";
        static string[] _CustomerRole = ConfigurationManager.AppSettings["CustomerRole"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        static string[] _EmployeeRole = ConfigurationManager.AppSettings["EmployeeRole"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        static void Main(string[] args)
        {
            Console.WriteLine("Start...");
            try
            {
                MongoDBHelper.InitMongodb();
                GetPageUrl();
                //Console.WriteLine("Start InitPermission...");
                //InitPermission();
                //InitClientAuthPermission();
                //Console.WriteLine("InitPermission End");

                Console.WriteLine("Start InitUser...");
                InitUser();
                Console.WriteLine("InitUser End");

                //Console.WriteLine("Start InitMenu...");
                //InitMenu();
                //InitClientMenu();
                //Console.WriteLine("InitMenu End");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            Console.WriteLine("End");
            Console.ReadKey();
        }
        /// <summary>
        /// 获取业务系统域名URL
        /// </summary>
        private static void GetPageUrl()
        {
            PortalApplicationManage appManage = new PortalApplicationManage();
            YwUrl = GetPageUrl(appManage, _ApplicationId);
            YwClientUrl = GetPageUrl(appManage, _ClientApplicationId);
        }

        private static string GetPageUrl(PortalApplicationManage appManage, string applicationId)
        {
            Application info = appManage.GetByKey(applicationId);
            if (info != null && !string.IsNullOrEmpty(info.Url))
            {
                return info.Url;
            }
            else
            {
                return ReplaceUrl;
            }
        }

        /// <summary>
        /// 业务系统用户初始化
        /// </summary>
        static void InitUser()
        {
            try
            {
                PortalUserManage portalUserManage = new ToolBLL.portal.PortalUserManage();
                PortalPermissionManage portalPermissionManage = new PortalPermissionManage();
                var portalPermissions = portalPermissionManage.GetPermission();
                //1：获取业务系统的所有用户信息
                var yewuUsers = new UserManage().GetUser();
                List<string> loginNames = new List<string>();
                //2：获取业务系统所有客户对应的权限信息
                var clientUserAuths = new UserManage().GetClientUserAuth();
                foreach (var item in yewuUsers)
                {
                    if (string.IsNullOrEmpty(item.LoginName))
                    {
                        log.Info("业务系统用户添加失败，失败原因：用户名不能为空");
                        continue;
                    }
                    if (loginNames.Contains(item.LoginName))
                    {
                        var users = yewuUsers.Where(s => s.LoginName == item.LoginName && s.IsApproved == true).ToList();
                        if (users.Count > 0)
                        {
                            var newportalUser = GetOldUser(users, clientUserAuths, portalPermissions, item);
                            portalUserManage.UpdateUser(newportalUser);
                        }
                        Console.WriteLine(string.Format("修复账号【{0}】，客户代码【{1}】的基本信息", item.LoginName, item.ClientNo));
                        log.Info("业务系统用户添加失败，失败原因：不能添加重复的用户名[" + item.LoginName + "]");
                        continue;
                    }
                    var oldUserInfo = portalUserManage.GetUserByLoginName(item.LoginName);
                    if (oldUserInfo != null)
                    {
                        var newportalUser2 = GetNewUser(item, clientUserAuths, portalPermissions);
                        newportalUser2._id = oldUserInfo._id;
                        newportalUser2.Permissions = oldUserInfo.Permissions;
                        newportalUser2.Roles = oldUserInfo.Roles;
                        //2：更新到portal
                        loginNames.Add(newportalUser2.LoginName);
                        portalUserManage.UpdateUser(newportalUser2);
                        Console.WriteLine(string.Format("修复已存在的账号【{0}】，客户代码【{1}】的基本信息", item.LoginName,item.ClientNo));
                        continue;
                    }
                    var newportalUser3 = GetNewUser(item, clientUserAuths, portalPermissions);
                    //2：更新到portal
                    loginNames.Add(newportalUser3.LoginName);
                    portalUserManage.InsertUser(newportalUser3);
                    Console.WriteLine(string.Format("导入账号【{0}】，客户代码【{1}】的基本信息", item.LoginName, item.ClientNo));
                }
            }
            catch (Exception ex)
            {
                log.Error("业务系统用户初始化失败。失败原因：" + ex.Message);
                log.Error(ex);
            }
        }

        private static User GetOldUser(List<ToolBLL.yewu.User> users, List<ClientUserAuth> clientUserAuths, List<Permission> portalPermissions, ToolBLL.yewu.User item)
        {
            var permissions = new List<string>();
            if (users[0].ClientID > 0)
            {
                var clientUserAuth = clientUserAuths.FirstOrDefault(s => s.UserID == users[0].UserID);
                if (clientUserAuth != null)
                {
                    permissions = clientUserAuth.Auths.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }
            else
            {
                permissions = users[0].Purview == null
                    ? new List<string>()
                    : users[0].Purview.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
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
            if (users[0].ClientID > 0)
            {
                roles.AddRange(_CustomerRole);
            }
            else
            {
                roles.AddRange(_EmployeeRole);
            }
            var newportalUser = new ToolBLL.portal.User()
            {
                LoginName = users[0].LoginName,
                Password = users[0].Password,
                DisplayName = users[0].DisplayName,
                Email = users[0].Email,
                Desc = users[0].Desc,
                EmployeeNo = users[0].EmployeeNo,
                ClientNo = item.ClientNo,
                IsApproved = users[0].IsApproved,
                CreatedOn = users[0].CreatedOn,
                LastLoginedIp = users[0].LastLoginedIp,
                LastLoginedTime = users[0].LastLoginedTime,
                UserType = users[0].ClientID > 0 ? UserType.Customer : UserType.Employee,
                Roles = roles,
                Permissions = newPermissions,
                _t = "User"
            };
            return newportalUser;
        }

        private static User GetNewUser(ToolBLL.yewu.User item, List<ClientUserAuth> clientUserAuths, List<Permission> portalPermissions)
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
                permissions = item.Purview == null
                    ? new List<string>()
                    : item.Purview.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
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
            return newportalUser;
        }

        /// <summary>
        /// 业务系统权限初始化
        /// </summary>
        static void InitPermission()
        {
            try
            {
                PermissionManage permissionManage = new PermissionManage();
                PortalPermissionManage portalPermissionManage = new PortalPermissionManage();
                List<string> permissionCodes = new List<string>();
                var childPermission = permissionManage.GetPermissionByDeeps(1);
                foreach (var item in childPermission)
                {
                    var t = "PagePermission";
                    var classSignArray = item.ClassSign.ToCharArray();
                    var classRemarkArray = item.ClassRemark.Split('|');
                    var functionPermissions = new List<string>();
                    var parentId = string.Empty;
                    for (int i = 0; i < classSignArray.Length; i++)
                    {

                        if (i >= classRemarkArray.Length)
                        {
                            continue;
                        }
                        var k = item.ClassKey.Trim(',');
                        if (classSignArray[i] != '0')
                        {
                            k += classSignArray[i];
                            t = "FunctionPermission";
                        }
                        else
                        {
                            foreach (var code in classSignArray)
                            {
                                if (code != '0')
                                {
                                    var pCode = k + code;
                                    functionPermissions.Add(pCode);
                                }
                            }
                        }
                        if (permissionCodes.Contains(k))
                        {
                            log.Info("业务系统权限码添加失败，失败原因：不能添加重复的权限码[" + k + "]");
                            continue;
                        }
                        var permission = new ToolBLL.portal.Permission()
                        {
                            ApplicationId = _ApplicationId,
                            _t = t,
                            Code = k,
                            Desc = classRemarkArray[i],
                            Order = i,
                            IsCompatible = true,
                            CreatedOn = DateTime.Now,
                            CreatedBy = "",
                            UpdatedOn = DateTime.Now,
                            UpdatedBy = ""
                        };
                        if (classSignArray[i] == '0')
                        {
                            parentId = permission._id;
                            permission.Name = item.ClassName;
                            permission.FunctionPermissions = functionPermissions;
                            permission.Tag = item.MenuList.ToString();
                        }
                        else
                        {
                            permission.Name = classRemarkArray[i];
                            permission.Tag = "" + classSignArray[i];
                            permission.ParentId = parentId;
                        }
                        permissionCodes.Add(permission.Code);
                        portalPermissionManage.InsertPermission(permission);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("业务系统权限初始化失败。失败原因：" + ex.Message);
                log.Error(ex);
            }
        }

        /// <summary>
        /// 业务系统客户权限初始化
        /// </summary>
        static void InitClientAuthPermission()
        {
            try
            {
                List<string> permissionCodes = new List<string>();
                PermissionManage permissionManage = new PermissionManage();
                PortalPermissionManage portalPermissionManage = new PortalPermissionManage();
                var permissions = permissionManage.GetClientAuths();
                foreach (var item in permissions)
                {
                    if (string.IsNullOrWhiteSpace(item.AuthKey))
                    {
                        continue;
                    }
                    if (permissionCodes.Contains(item.AuthKey))
                    {
                        log.Info("业务系统权限码添加失败，失败原因：不能添加重复的权限码[" + item.AuthKey + "]");
                        continue;
                    }
                    var classRemarkArray = item.AuthChilds.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    var functionPermissions = new List<string>();
                    var parentId = string.Empty;
                    //1：添加页面权限
                    for (int j = 0; j < classRemarkArray.Length; j++)
                    {
                        var pCode = item.AuthKey + j;
                        functionPermissions.Add(pCode);
                    }
                    var parentPermission = new ToolBLL.portal.Permission()
                    {
                        ApplicationId = _ClientApplicationId,
                        _t = "PagePermission",
                        Code = item.AuthKey,
                        Name = item.Name,
                        Order = item.SortOrder,
                        FunctionPermissions = functionPermissions,
                        IsCompatible = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = "",
                        UpdatedOn = DateTime.Now,
                        UpdatedBy = ""
                    };
                    parentId = parentPermission._id;
                    permissionCodes.Add(parentPermission.Code);
                    portalPermissionManage.InsertPermission(parentPermission);

                    //2:添加页面功能权限
                    for (int i = 0; i < classRemarkArray.Length; i++)
                    {
                        if (i >= classRemarkArray.Length)
                        {
                            continue;
                        }
                        if (permissionCodes.Contains(item.AuthKey + i))
                        {
                            log.Info("业务系统权限码添加失败，失败原因：不能添加重复的权限码[" + (item.AuthKey + i) + "]");
                            continue;
                        }
                        var functionPermission = new ToolBLL.portal.Permission()
                        {
                            ApplicationId = _ClientApplicationId,
                            _t = "FunctionPermission",
                            Code = item.AuthKey + i,
                            Name = classRemarkArray[i],
                            Desc = classRemarkArray[i],
                            IsCompatible = true,
                            ParentId = parentId,
                            Order = i,
                            CreatedOn = DateTime.Now,
                            CreatedBy = "",
                            UpdatedOn = DateTime.Now,
                            UpdatedBy = ""
                        };
                        permissionCodes.Add(functionPermission.Code);
                        portalPermissionManage.InsertPermission(functionPermission);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("业务系统权限初始化失败。失败原因：" + ex.Message);
                log.Error(ex);
            }
        }

        /// <summary>
        /// 业务系统菜单初始化
        /// </summary>
        static void InitMenu()
        {
            try
            {
                PermissionManage permissionManage = new PermissionManage();
                //PortalPermissionManage portalPermissionManage = new PortalPermissionManage();
                PortalMenuManage menuManage = new PortalMenuManage();
                var parentMenu = permissionManage.GetPermissionByDeeps(0);
                var childMenu = permissionManage.GetPermissionByDeeps(1);
                //创建系统级菜单  业务系统
                var yewumenu = new Menu()
                {
                    Name = "业务系统",
                    Sequence = 100,
                    PermissionCode = string.Empty,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    _t = "Menu"
                };
                //插入业务系统 主菜单
                menuManage.InsertMenu(yewumenu);

                foreach (var item in parentMenu)
                {
                    var menu = new Menu()
                    {
                        Name = item.ClassName,
                        Sequence = item.MenuList,
                        ParentId = yewumenu._id,
                        PermissionCode = string.Empty,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        _t = "Menu"
                    };
                    //创建一级菜单
                    menuManage.InsertMenu(menu);
                    var childMenusByParent = childMenu.Where(s => s.ParentID == item.ClassID);
                    foreach (var second in childMenusByParent)
                    {
                        //创建二级菜单
                        menuManage.InsertMenu(new Menu()
                            {
                                Name = second.ClassName,
                                ParentId = menu._id,
                                Url = string.Format("{0}/{1}", YwUrl, second.LinkTo.Replace(ReplaceUrl, "")),
                                Sequence = second.MenuList,
                                PermissionCode = second.ClassKey.Trim(','),
                                CreatedOn = DateTime.Now,
                                UpdatedOn = DateTime.Now,
                                _t = "Menu"
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("业务系统权限初始化失败，失败原因：" + ex.Message);
                log.Error(ex);
            }
        }
        /// <summary>
        /// 业务系统菜单初始化
        /// </summary>
        static void InitClientMenu()
        {
            try
            {
                PermissionManage permissionManage = new PermissionManage();
                PortalPermissionManage portalPermissionManage = new PortalPermissionManage();
                PortalMenuManage menuManage = new PortalMenuManage();
                var parentMenu = permissionManage.GetClientMenus(0);
                var childMenu = permissionManage.GetClientMenus(1);
                var permissions = permissionManage.GetClientAuths();
                //创建系统级菜单  业务系统
                var yewumenu = new Menu()
                {
                    Name = "业务前端系统",
                    Sequence = 200,
                    PermissionCode = string.Empty,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    _t = "Menu"
                };
                //插入业务系统 主菜单
                menuManage.InsertMenu(yewumenu);
                //  new MenuItem{Name="仓储服务", ItemsId = new string[]{"1000","1100","1200"}},
                AddClientMenu(menuManage, parentMenu, childMenu, yewumenu, "仓储服务", new int[] { 1000, 1100, 1200 }, 1, permissions);
                //new MenuItem{Name="FBA服务", ItemsId = new string[]{"1500"}},
                AddClientMenu(menuManage, parentMenu, childMenu, yewumenu, "FBA服务", new int[] { 1500 }, 2, permissions);
                //new MenuItem{Name="直发服务", ItemsId = new string[]{"2000"}},
                AddClientMenu(menuManage, parentMenu, childMenu, yewumenu, "直发服务", new int[] { 2000 }, 3, permissions);
                //new MenuItem{Name="仓库",     ItemsId = new string[]{"3000","3100"}},
                AddClientMenu(menuManage, parentMenu, childMenu, yewumenu, "仓库", new int[] { 3000, 3100 }, 4, permissions);
                //new MenuItem{Name="工具",     ItemsId = new string[]{"4000","4100"}},
                AddClientMenu(menuManage, parentMenu, childMenu, yewumenu, "工具", new int[] { 4000, 4100 }, 5, permissions);
                //new MenuItem{Name="财务",     ItemsId = new string[]{"5000","5100"}},
                AddClientMenu(menuManage, parentMenu, childMenu, yewumenu, "财务", new int[] { 5000, 5100 }, 6, permissions);
                //new MenuItem{Name="计价查询", ItemsId = new string[]{"6000"}},
                AddClientMenu(menuManage, parentMenu, childMenu, yewumenu, "计价查询", new int[] { 6000 }, 7, permissions);
                //new MenuItem{Name="保险服务", ItemsId = new string[]{"1400"}},
                AddClientMenu(menuManage, parentMenu, childMenu, yewumenu, "保险服务", new int[] { 1400 }, 8, permissions);
                //new MenuItem{Name="交货",     ItemsId = new string[]{"7000","7100","7200"}},
                AddClientMenu(menuManage, parentMenu, childMenu, yewumenu, "交货", new int[] { 7000, 7100, 7200 }, 9, permissions);
                //new MenuItem{Name="包装材料", ItemsId = new string[]{"8000"}},
                AddClientMenu(menuManage, parentMenu, childMenu, yewumenu, "包装材料", new int[] { 8000 }, 10, permissions);
                //new MenuItem{Name="账号",     ItemsId = new string[]{"9000","9100"}}
                AddClientMenu(menuManage, parentMenu, childMenu, yewumenu, "账号", new int[] { 9000, 9100, }, 11, permissions);
            }
            catch (Exception ex)
            {
                log.Error("业务系统权限初始化失败，失败原因：" + ex.Message);
                log.Error(ex);
            }
        }

        private static void AddClientMenu(PortalMenuManage menuManage, List<ClientMenu> parentMenu, List<ClientMenu> childMenu, Menu yewumenu, string name, int[] itemsId, int sequence, List<ClientAuth> permissions)
        {
            if (itemsId.Length < 1) return;
            var menu1 = new Menu()
            {
                Name = name,
                Sequence = sequence,
                ParentId = yewumenu._id,
                PermissionCode = string.Empty,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                _t = "Menu"
            };
            //创建一级菜单
            menuManage.InsertMenu(menu1);
            foreach (int itemId in itemsId)
            {
                var childMenusBySortOrder = parentMenu.Where(s => s.SortOrder == itemId).ToList();
                foreach (var second in childMenusBySortOrder)
                {
                    var secondMenu = new Menu()
                    {
                        Name = second.Name,
                        ParentId = menu1._id,
                        Url = second.LinkUrl,
                        Sequence = second.SortOrder,
                        PermissionCode = string.Empty,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        _t = "Menu"
                    };
                    //创建二级菜单
                    menuManage.InsertMenu(secondMenu);
                    var childMenusByParent = childMenu.Where(s => s.Parent == second.ID).ToList();
                    foreach (var three in childMenusByParent)
                    {
                        var permission = permissions.FirstOrDefault(s => s.ID == three.Auth);
                        var threeMenu = new Menu()
                        {
                            Name = three.Name,
                            ParentId = secondMenu._id,
                            Url = string.Format("{0}/{1}", YwClientUrl, three.LinkUrl.Replace(ReplaceUrl, "")),
                            Sequence = three.SortOrder,
                            PermissionCode = permission == null ? string.Empty : permission.AuthKey,
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now,
                            _t = "Menu"
                        };
                        //创建三级菜单
                        menuManage.InsertMenu(threeMenu);
                    }
                }
            }
        }


    }
}
