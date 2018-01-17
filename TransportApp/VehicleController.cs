using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportApp.DbCore;
using TransportApp.Models;
using Newtonsoft.Json;
using System;

namespace TransportApp
{
    [Route("api/Vehicle")]
    public class VehicleController : Controller
    {

        private readonly RepoContext Context;

        public VehicleController(RepoContext context)
        {
            Context = context;
        }

        [HttpPost]
        public void Post([FromBody]Vehicle inmodel)
        {

            inmodel.Id = Context.Vehicle.Max(x => x.Id) + 1;


            Context.Vehicle.Add(inmodel);
            Context.SaveChanges();
        }

        [HttpPost]
        [Route("addDriver")]
        public void AddDriver([FromBody]Driver inmodel)
        {

            inmodel.Id = Context.Driver.Max(x => x.Id) + 1;

            Context.Driver.Add(inmodel);
            Context.SaveChanges();
        }



    }
}
