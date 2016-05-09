using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBLL.yewu
{
    public class ClientAuth
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AuthKey { get; set; }

        public string AuthChilds { get; set; }

        public int SortOrder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Parent { get; set; }

    }
}
