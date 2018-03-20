using CICSRemittanceService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;

namespace CICSRemittanceService.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/<controller>
        public async Task<HttpResponseMessage> Get()
        {
            var stream = await Request.Content.ReadAsStreamAsync();

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);

            // Process XML document

            return new HttpResponseMessage();
        }

        // GET api/<controller>/5
        public string Get(ValidationRequest req)
        {
            var obj = req;
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}