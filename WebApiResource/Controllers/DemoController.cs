using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Portal.WebApi;

namespace WebApiResource.Controllers
{
    [DemoAuthFilter("Get_Inventory")]
    public class DemoController : ApiController
    {
        public string Get()
        {
            //return "hello";
            var id = (AccessTokenIdentity) this.User.Identity;
            return string.Format("customer no:{0}, customer name:{1}, app clientid:{2}", id.CustomerNo, id.Name, id.AppClientId);
        }
    }
}
