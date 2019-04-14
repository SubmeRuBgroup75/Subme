using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SubMe.Models;

namespace SubMe.Controllers
{
    public class UserProfileController : ApiController
    {

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] UserProfile NewP)
        {
            UserProfile uProfile = new UserProfile();
            uProfile.InsertUserProfile(NewP);
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
