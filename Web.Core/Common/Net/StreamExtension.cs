using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Web.Core
{
    public static class StreamExtension
    {
        #region 根据指定编码将流解读为字符串
        /// <summary>
        /// 根据指定编码将流解读为字符串
        /// </summary>
        /// <param name="stream">待解析的流</param>
        /// <param name="encode">流的编码</param>
        /// <returns>字符串</returns>
        public static string ToString(this Stream stream, Encoding encode)
        {
            using (var myStreamReader = new StreamReader(stream, encode))
            {
                return myStreamReader.ReadToEnd();
            }
        }
        #endregion

        #region 将普通流转换成内存流
        /// <summary>
        /// 将普通流转换成内存流
        /// </summary>
        /// <param name="stream">普通流</param>
        /// <returns>内存流</returns>
        public static MemoryStream ToMemoryStream(this Stream stream)
        {
            if (stream is MemoryStream) return stream as MemoryStream;

            using (stream)
            {
                var buffer = new byte[1024];
                var memoryStream = new MemoryStream();
                int i = 1;
                while (i > 0)
                {
                    i = stream.Read(buffer, 0, buffer.Length);
                    memoryStream.Write(buffer, 0, i);
                }
                memoryStream.Seek(0, SeekOrigin.Begin);
                return memoryStream;
            }
        }

        #endregion

        #region 将流写入文件
        /// <summary>
        /// 将流写入文件
        /// </summary>
        /// <param name="stream">待写入流</param>
        /// <param name="path">目标文件路径</param>
        public static void Save(this Stream stream, string path)
        {
            using (var fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                var buffer = new byte[1024];
                int length = int.MaxValue;
                while (length > 0)
                {
                    length = stream.Read(buffer, 0, buffer.Length);
                    fs.Write(buffer, 0, length);
                }
            }
        }
        #endregion

    }
}
