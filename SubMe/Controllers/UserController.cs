using SubMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SubMe.Controllers
{
    public class UserController : ApiController
    {
        // POST api/<controller>
        public void Post([FromBody]User u)
        {
            u.insert();
        }

        // GET api/<controller>/5
        public int Get(string fbid)
        {
            User u = new User();
            return u.CheckUserExist(fbid);
        }

        // GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}