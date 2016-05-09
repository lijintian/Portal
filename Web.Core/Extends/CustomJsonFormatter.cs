using EasyDDD.Infrastructure.Crosscutting.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace Portal.Web.Core.Extends
{
    /// <summary>
    /// 自定义JsonFormatter
    /// </summary>
    public class CustomJsonFormatter : MediaTypeFormatter
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomJsonFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedEncodings.Add(new UTF8Encoding(false, true));
            SupportedEncodings.Add(new UnicodeEncoding(false, true, true));
        }



        #region Overrides of MediaTypeFormatter

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.Factory.StartNew(() =>
            {
                var result = JsonManager.DeserializeFromStream(type, readStream);
                return result;
            });
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
            {
                JsonManager.SerializeToStream(value, type, writeStream);
            });
        }

        /// <summary>
        /// Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter"/> can deserializean object of the specified type.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter"/> can deserialize the type; otherwise, false.
        /// </returns>
        /// <param name="type">The type to deserialize.</param>
        public override bool CanReadType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return true;
        }

        /// <summary>
        /// Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter"/> can serializean object of the specified type.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter"/> can serialize the type; otherwise, false.
        /// </returns>
        /// <param name="type">The type to serialize.</param>
        public override bool CanWriteType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return true;
        }

        #endregion
    }

}
