using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace Portal.Web.Core.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        [HttpGet]
        [ResponseType(typeof(Test))]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {

            var test = new Test();
            if (id == 1) test.Time = DateTime.UtcNow;
            else if (id == 2) test.Time = DateTime.MaxValue;
            else if (id == 3) test.Time = DateTime.Now;
            else if (id == 4) test.Time = new DateTime(DateTime.MaxValue.Year, DateTime.MaxValue.Month, DateTime.MaxValue.Day, DateTime.MaxValue.Hour, DateTime.MaxValue.Minute, DateTime.MaxValue.Second, DateTimeKind.Local);
            return Ok(test);
        }

    }

    public class Test
    {
        public DateTime Time { get; set; }
    }
}
