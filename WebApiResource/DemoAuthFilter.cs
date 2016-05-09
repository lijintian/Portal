using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using Portal.WebApi;

namespace WebApiResource
{
    public class DemoAuthFilter : PortalAccessTokenAuthenticationFilter
    {
        public DemoAuthFilter(string code) : base(code)
        {
            
        }

        protected override IHttpActionResult GetChallenge(HttpAuthenticationChallengeContext context, string error)
        {
            return base.GetChallenge(context, error);
        }
    }
}