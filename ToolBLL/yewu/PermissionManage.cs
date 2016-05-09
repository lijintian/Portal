
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace ToolBLL.yewu
{
    public class PermissionManage
    {
        public List<Permission> GetPermissionByDeeps(int deeps)
        {
            List<Permission> list = new List<Permission>();
            string sql = "select ClassID,ClassName,ClassKey,ClassRemark,ClassSign,ParentID,MenuList,Deeps,DealFlag,LinkTo from Sys_Class where Deeps=" + deeps + " and DealFlag=0 and MenuList>0 order by MenuList asc";
            SqlDataReader reader = SQLHelper.GetReader(sql);
            while (reader.Read())
            {
                var permission = new Permission();
                permission.ClassID = Convert.ToInt64(reader["ClassID"]);
                permission.ClassName = reader["ClassName"] as string;
                permission.ClassKey = reader["ClassKey"] as string;
                permission.ClassRemark = reader["ClassRemark"] as string;
                permission.ClassSign = reader["ClassSign"] as string;
                permission.LinkTo = reader["LinkTo"] as string;
                permission.ParentID = Convert.ToInt64(reader["ParentID"]);
                permission.MenuList = Convert.ToInt32(reader["MenuList"]);
                permission.Deeps = Convert.ToInt32(reader["Deeps"]);
                permission.DealFlag = Convert.ToInt32(reader["DealFlag"]);
                list.Add(permission);
            }
            return list;
        }

        public int UpdatePermission(Permission permission)
        {
            int result = 0;
            string sql = string.Format("update Sys_Class set ClassName='{0}',ClassKey='{1}',ClassRemark='{2}',ParentID={3},MenuList={4},Deeps={5},DealFlag={6},ClassSign='{7}' where ClassID={8}",
                permission.ClassName, permission.ClassKey, permission.ClassRemark, permission.ParentID, permission.MenuList, permission.Deeps, permission.DealFlag, permission.ClassSign, permission.ClassID);
            result = SQLHelper.ExecNoQuery(sql);
            return result;
        }

        public int InsertPermission(Permission permission)
        {
            int result = 0;
            string sql = string.Format("insert Sys_Class (ClassName,ClassKey,ClassRemark,ParentID,MenuList,Deeps,DealFlag,ClassSign,WWW) values('{0}','{1}','{2}',{3},{4},{5},{6},'{7}','localhost')",
                 permission.ClassName, permission.ClassKey, permission.ClassRemark, permission.ParentID, permission.MenuList, permission.Deeps, permission.DealFlag, permission.ClassSign);
            result = SQLHelper.ExecNoQuery(sql);
            return result;
        }

        public List<ClientAuth> GetClientAuths()
        {
            List<ClientAuth> list = new List<ClientAuth>();
            string sql = "select ID,Parent,Name,AuthKey,AuthChilds,SortOrder from Client_Auth where parent!=0  order by SortOrder asc";
            SqlDataReader reader = SQLHelper.GetReader(sql);
            while (reader.Read())
            {
                var clientAuth = new ClientAuth();
                clientAuth.ID = Convert.ToInt32(reader["ID"]);
                clientAuth.Name = reader["Name"] as string;
                clientAuth.AuthKey = reader["AuthKey"] as string;
                clientAuth.AuthChilds = reader["AuthChilds"] as string;
                clientAuth.Parent = Convert.ToInt32(reader["Parent"]);
                clientAuth.SortOrder = Convert.ToInt32(reader["SortOrder"]);
                list.Add(clientAuth);
            }
            return list;
        }

        public List<ClientMenu> GetClientMenus(int parent)
        {
            List<ClientMenu> list = new List<ClientMenu>();
            string sql = string.Empty;
            if (parent == 0)
            {
                sql = "select * from client_menu where parent=0 and MenuState=1";
            }
            else
            {
                sql = "select * from client_menu where parent!=0 and MenuState=1";
            }
            SqlDataReader reader = SQLHelper.GetReader(sql);
            while (reader.Read())
            {
                var clientAuth = new ClientMenu();
                clientAuth.ID = Convert.ToInt32(reader["ID"]);
                clientAuth.Name = reader["Name"] as string;
                clientAuth.Auth = Convert.ToInt32(reader["Auth"]);
                clientAuth.LinkUrl = reader["LinkUrl"] as string;
                clientAuth.Parent = Convert.ToInt32(reader["Parent"]);
                clientAuth.SortOrder = Convert.ToInt32(reader["SortOrder"]);
                list.Add(clientAuth);
            }
            return list;
        }

    }
}
