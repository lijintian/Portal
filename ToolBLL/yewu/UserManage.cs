
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace ToolBLL.yewu
{
    public class UserManage
    {
        public List<User> GetUser()
        {
            string sql = "select UserID,LoginName,UserPwd,Remark,TrueName,Email,LoginID,DealFlag,LastLogin,LastLoginIP,InputTime,Purview,ClientID,(select top 1 ClientNo from Crm_Client t2 where  t2.ClientId=t1.ClientID ) ClientNo from hr_users t1";
            return MapList(sql);
        }

        public List<User> GetUser2()
        {
            string sql = "select UserID,LoginName,UserPwd,Remark,TrueName,Email,LoginID,DealFlag,LastLogin,LastLoginIP,InputTime,Purview,ClientID,(select top 1 ClientNo from Crm_Client t2 where  t2.ClientId=t1.ClientID ) ClientNo from hr_users t1 where [InputTime]>'2016-01-17' and DealFlag='正常'";
            return MapList(sql);
        }

        public User GetUserByLoginName(string loginName)
        {
            string sql = "select top 1 UserID,LoginName,UserPwd,Remark,TrueName,Email,LoginID,DealFlag,LastLogin,LastLoginIP,InputTime,ClientID from hr_users where LoginName='" + loginName + "'";
            SqlDataReader reader = SQLHelper.GetReader(sql);
            var user = new User();
            while (reader.Read())
            {
                user.UserID = Convert.ToInt64(reader["UserID"]);
                user.LoginName = reader["LoginName"] as string;
                user.Password = reader["UserPwd"] as string;
                user.Desc = reader["Remark"] as string;
                user.DisplayName = reader["TrueName"] as string;
                user.Email = reader["Email"] as string;
                user.EmployeeNo = (reader["LoginID"] is DBNull) ? string.Empty: Convert.ToInt32(reader["LoginID"]).ToString();
                user.IsApproved = (string)reader["DealFlag"] == "正常" ? true : false;
                user.LastLoginedTime = reader["LastLogin"] as DateTime?;
                user.LastLoginedIp = reader["LastLoginIP"] as string;
                user.CreatedOn = (DateTime)reader["InputTime"];
                user.ClientID = Convert.ToInt32(reader["ClientID"]);
            }
            return user;
        }

        public int UpdateUserPermission(string permissions, string loginName)
        {
            int result = 0;
            string sql = string.Format("update hr_users set purview='{0}' where LoginName='{1}'", permissions, loginName);
            result = SQLHelper.ExecNoQuery(sql);
            return result;
        }
        public int UpdateUser(User user)
        {
            int result = 0;
            string sql = string.Format("update hr_users set UserPwd='{0}',Remark='{1}',TrueName='{2}',Email='{3}',DealFlag='{4}' where LoginName='{5}'",
                user.Password, user.Desc, user.DisplayName, user.Email, user.IsApproved ? "正常" : "停用", user.LoginName);
            result = SQLHelper.ExecNoQuery(sql);
            return result;
        }
        public int UpdateClient(User user)
        {
            int result = 0;
            string sql = string.Format("update crm_client set ClientPwd='{0}',Remark='{1}',Email='{2}',DealFlag='{3}',ClientName='{4}' where ClientID={5}",
                user.Password, user.Desc, user.Email, user.IsApproved ? "正常" : "取消", user.DisplayName, user.ClientID);
            result = SQLHelper.ExecNoQuery(sql);
            return result;
        }
        public int UpdateUserPermission(User user)
        {
            int result = 0;
            string sql = string.Format("update hr_users set Purview='{0}' where LoginName='{1}'",
                user.Purview, user.LoginName);
            result = SQLHelper.ExecNoQuery(sql);
            return result;
        }

        public bool HasUserAuthByUserID(long userID)
        {
            string sql = "select count(*) as has from client_UserAuth where UserID=" + userID + "";
            SqlDataReader reader = SQLHelper.GetReader(sql);
            while (reader.Read())
            {
                if (Convert.ToInt32(reader["has"]) > 0)
                    return true;
            }
            return false;
        }
        public List<ClientUserAuth> GetClientUserAuth()
        {
            List<ClientUserAuth> list = new List<ClientUserAuth>();
            string sql = "select UserID,Auths from client_UserAuth";
            SqlDataReader reader = SQLHelper.GetReader(sql);
            while (reader.Read())
            {
                var user = new ClientUserAuth();
                user.UserID = Convert.ToInt64(reader["UserID"]);
                user.Auths = reader["Auths"] as string;
                list.Add(user);
            }
            return list;
        }
        public int UpdateClientUserAuth(ClientUserAuth info)
        {
            int result = 0;
            string sql = string.Format("update client_UserAuth set Auths='{0}' where UserID={1}",
                info.Auths, info.UserID);
            result = SQLHelper.ExecNoQuery(sql);
            return result;
        }
        public int InsertClientUserAuth(ClientUserAuth info)
        {
            int result = 0;
            string sql = string.Format("insert client_UserAuth ([UserID],[Auths]) VALUES({0},'{1}')",
                info.UserID, info.Auths);
            result = SQLHelper.ExecNoQuery(sql);
            return result;
        }

        #region 私有方法
        private List<User> MapList(string sql)
        {
            List<User> list = new List<User>();
            SqlDataReader reader = SQLHelper.GetReader(sql);
            while (reader.Read())
            {
                list.Add(Map(reader));
            }
            return list;
        }

        private User Map(SqlDataReader reader)
        {
            var user = new User();
            user.UserID = Convert.ToInt64(reader["UserID"]);
            user.LoginName = reader["LoginName"] as string;
            user.Password = reader["UserPwd"] as string;
            user.Desc = reader["Remark"] as string;
            user.DisplayName = reader["TrueName"] as string;
            user.Email = reader["Email"] as string;
            user.EmployeeNo = (reader["LoginID"] is DBNull) ? string.Empty : Convert.ToInt32(reader["LoginID"]).ToString();
            user.ClientNo = reader["ClientNo"] as string;
            user.IsApproved = (string)reader["DealFlag"] == "正常" ? true : false;
            user.LastLoginedTime = reader["LastLogin"] as DateTime?;
            user.LastLoginedIp = reader["LastLoginIP"] as string;
            user.Purview = reader["Purview"] as string;
            user.CreatedOn = (DateTime)reader["InputTime"];
            user.ClientID = Convert.ToInt32(reader["ClientID"]);
            return user;
        }
        #endregion
    }
}
