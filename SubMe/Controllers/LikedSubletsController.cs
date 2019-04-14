using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SubMe.Models;

namespace SubMe.Controllers
{
    public class LikedSubletsController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(string UserFBID)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]LikedSublets[] ls)
        {
            LikedSublets l = new LikedSublets();
            l.UpdateLikedSublets(ls);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete([FromBody]LikedSublets ls)
        {
            LikedSublets l = new LikedSublets();
            l.DeleteLikedSublet(ls);
        }
    }
}