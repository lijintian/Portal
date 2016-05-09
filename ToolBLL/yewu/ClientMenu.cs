using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBLL.yewu
{
    public class ClientMenu
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        public string LinkUrl { get; set; }

        public int SortOrder { get; set; }
        public int Auth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Parent { get; set; }
    }
}
