using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Web.Core.Model
{
    public class FileTypes
    {
        private static Dictionary<string, string> list = new Dictionary<string, string>
            {
                            {"xls", "application/ms-excel"},
                            {"xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                            {"doc", "application/msword"},
                            {"docx", "application/msword"},
                            {"zip", "application/zip"},
                            {"txt", "application/x-tex"},
                            {"xml", "application/xml"},
                            {"stream", "application/octet-stream"},
                        };

        public static string GetContentType(string fileExt)
        {
            fileExt = fileExt.ToLower();
            return list.ContainsKey(fileExt) ? list[fileExt] : "application/" + fileExt;
        }
    }
}
