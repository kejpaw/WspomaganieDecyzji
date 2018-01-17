using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportApp.DbCore;
using TransportApp.Models;
using TransportApp.Helpers;
using Newtonsoft.Json;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportApp
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        private readonly RepoContext Context;
        
        public ValuesController(RepoContext context)
        {
            Context = context;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {

            //var orderek = Context.Order.First();
            TestTable tejbl;
            Orders orderek;


                tejbl = Context.TestTableRepo.First();
                var ordery = Context.Orders.ToList();
                orderek = Context.Orders.First();
                Console.Write("ddd");


            return new string[] { $"value1", "value2" };
        }

        [HttpPost]
        public void Post([FromBody]Orders inmodel)
        {
            inmodel.Id = Context.Orders.Max(x => x.Id) + 1;

            inmodel.StartDescription = string.Empty;
            inmodel.EndDescription = string.Empty;

            try
            {
                Context.Orders.Add(inmodel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            Context.SaveChanges();
            GoogleMatrixHelper.AddPointToMatrix(Context, inmodel);
            Context.SaveChanges();
        }

        [HttpGet]
        [Route("GetMarkers")]
        public string GetMarkers()
        {
            List<Orders> ordersy = new List<Orders>();

            try
            {
                ordersy = Context.Orders.ToList();
            }
            catch (Exception ex)
            {
                Console.Write($"{ex.Message}");
            }
            return JsonConvert.SerializeObject(Context.Orders.ToList()); 
        }


        [HttpGet]
        [Route("GetMatrix")]
        public string GetMatrix()
        {
            string dd= "nic";

            DistanceMatrix matrix;

            return dd;
        }



        [HttpPost]
        [Route("GetAstar")]
        public string GetAstar([FromBody] AStarParameters  parameters)
        {
            var astar = new AStarAlgorithm(Context, parameters);

            var used = astar.CountAlgorithm();

            foreach (var order in Context.Orders.Where(x => used.Contains(x.Id)))
                {
                order.IsInProgress = true;
                }

            return JsonConvert.SerializeObject(Context.Orders.ToList());
        }


        [HttpGet]
        [Route("ClearAll")]
        public string ClearAll()
        {
            Context.Distance.RemoveRange(Context.Distance.ToList());
            Context.Orders.RemoveRange(Context.Orders.ToList());

            Context.SaveChanges();

            return "All deleted";
        }


        [HttpGet]
        [Route("GetDirections")]
        public string GetDirections()
        {
   
            return NavigationLogger.BuildNavigationMessage(Context.Orders.ToList());
        }

    }
}
