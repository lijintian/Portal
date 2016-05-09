using System.Web;
using System.Web.Mvc;
using Portal.Applications.Services.Impl;
using Portal.Dto.Request.Enum;
using Portal.Web.Admin.Core;
using Portal.Web.Core;
using Portal.Web.Core.Model;

namespace Portal.Web.Admin.Controllers
{
    public class FileController : BaseController
    {
        public ActionResult BatchImport(int type)
        {
            ViewBag.TypeId = (TemplateType)type;
            return PartialView();
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <returns></returns>
        public string ImportSave(TemplateType type)
        {
            if (Request.Files.Count <= 0)
            {
                return ReturnJson("请上传附件！", false);
            }
            HttpPostedFileBase file = Request.Files[0];
            ReturnModel<string> result = ImportManagerService.Execute(file, type, PageUtility.GetLogger());
            return result.ToJson();
        }

        /// <summary>
        /// 根据类型下载模板
        /// </summary>
        public void DownTemplate(TemplateType type)
        {
            string path = ImportManagerService.GetFilePath(type);
            ExportHelper.Excel(path, string.Format("{0}模板{1}", type.GetDescription(), FileExtent.GetFileExt(path)));
        }

        /// <summary>
        /// 下载错误数据附件
        /// </summary>
        public void DownErrorFile(string path)
        {
            ExportHelper.Export(path, string.Format("错误数据{0}", FileExtent.GetFileExt(path)));
        }
    }
}
