
namespace ToolBLL.yewu
{
    public class Permission
    {
        /// <summary>
        /// 
        /// </summary>
        public long ClassID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClassKey { get; set; }

        public string ClassRemark { get; set; }
        public string ClassSign { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long ParentID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int MenuList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Deeps { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int DealFlag { get; set; }

        public string LinkTo { get; set; }

    }
}
