using SubMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SubMe.Controllers
{
    public class CityBelongingController : ApiController
    {

        // GET api/<controller>/5
        public CityBelonging Get(string id)
        {
            CityBelonging getCB = new CityBelonging();
            getCB = getCB.ReturnOneUserCityBelonging(id);
            return getCB;
        }

        // POST api/<controller>
        public void Post([FromBody]CityBelonging newCB)
        {
            CityBelonging CB = new CityBelonging();
            CB.InsertCityBelonging(newCB);
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
