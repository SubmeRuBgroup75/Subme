using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SubMe.Models;

namespace SubMe.Controllers
{
    public class UserPreferencesController : ApiController
    {
        // GET api/<controller>
        public List <UserPreferences> GetListOfSearchAttribute(string uid)
        {
            UserPreferences up = new UserPreferences();
            return up.GetListOfSearchAttribute(uid);
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]UserPreferences NewP)
        {
            UserPreferences up = new UserPreferences();
            up.InsertUserPreferences(NewP);
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