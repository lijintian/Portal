using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Dto.Common
{
    [DataContract]
    public class ErrorData
    {
        [DataMember]
        public string TicketId { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public DateTime DateTime { get; set; }
        [DataMember]
        public Uri RequestUri { get; set; }
        [DataMember]
        public string ErrorCode { get; set; }


        public ErrorData()
        {
            DateTime = DateTime.UtcNow;
        }

        public ErrorData(string errorCode, string errorMessage)
            : this(errorCode, errorMessage, null)
        {

        }

        public ErrorData(string errorCode, string errorMessage, Uri requestUri)
            : this()
        {

            ErrorCode = errorCode;
            Message = errorMessage;
            RequestUri = requestUri;
        }
    }
}
