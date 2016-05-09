using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Client.Model
{
    public static class MenuCodes
    {
        public static List<SysMenuInfo> GetMenuList(string code)
        {
            List<SysMenuInfo> list = new List<SysMenuInfo>();
            SysMenuInfo user = new SysMenuInfo("账号管理");
            user.Childs.Add(new SysMenuInfo("新建账号", "UserCreate", code));
            user.Childs.Add(new SysMenuInfo("登录", "Login", code));
            user.Childs.Add(new SysMenuInfo("注销", "Logout", code));
            user.Childs.Add(new SysMenuInfo("查看账号", "UserView", code));
            user.Childs.Add(new SysMenuInfo("修改账号", "UserEdit", code));
            user.SetClassName();
            list.Add(user);
            SysMenuInfo app = new SysMenuInfo("应用系统管理");
            app.Childs.Add(new SysMenuInfo("查看我的应用", "AppList", code));
            app.Childs.Add(new SysMenuInfo("新建应用", "AppCreate", code));
            app.Childs.Add(new SysMenuInfo("修改应用", "AppEdit", code));
            app.Childs.Add(new SysMenuInfo("提交审核", "AppSubmit", code));
            app.SetClassName();
            list.Add(app);
            SysMenuInfo api = new SysMenuInfo("出口易API手册");
            api.Childs.Add(new SysMenuInfo("了解OAuth2.0", "OAuth2", code));
            api.Childs.Add(new SysMenuInfo("开发者流程", "Flow", code));
            api.SetClassName();
            list.Add(api);
            return list;
        }
    }
}
