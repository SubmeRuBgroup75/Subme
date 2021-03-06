﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SubMe.Models;

namespace SubMe.Controllers
{
    public class LocationController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Location> Get()
        {
            Location L = new Location();
            return L.GetLocations();
        }

        // GET api/<controller>/5
        public List<Location> Get(string IdString)
        {
            Location L = new Location();
            return L.GetSearchedLocations(IdString);
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