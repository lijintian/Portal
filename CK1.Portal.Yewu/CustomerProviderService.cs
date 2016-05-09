using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Model;
using Portal.Domain.Services;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Portal.Yewu
{
    /// <summary>
    /// 表示从业务系统获取客户信息实现
    /// </summary>
    public class CustomerProviderService : ICustomerProviderService
    {
        private const string QueryCustomerSql = "SELECT  ClientID AS CustomerId, ClientName AS CustomerName, ClientNo AS CustomerNo FROM [dbo].[Crm_Client]"
            + " WHERE ClientID = (SELECT TOP 1 ClientID FROM dbo.HR_Users WHERE LoginName = @loginName)";
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["YewuDB"].ConnectionString;
        public CustomerInfo GetCustomerInfoByLoginName(string loginName)
        {
            if (string.IsNullOrEmpty(loginName))
            {
                return null;
            }

            Database db = new SqlDatabase(ConnectionString);
            var accessOr = db.CreateSqlStringAccessor<CustomerInfo>(QueryCustomerSql, new CustomerParameterMapper());
            var result = accessOr.Execute(new object[]{ loginName});

            return result.FirstOrDefault();
        }
    }

    class CustomerParameterMapper : IParameterMapper
    {
        public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
        {
            var para = command.CreateParameter();
            para.ParameterName = "@loginName";
            para.Value = parameterValues[0];
            command.Parameters.Add(para);
        }
    }
}
