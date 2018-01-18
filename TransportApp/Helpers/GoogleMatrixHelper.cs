using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Net.Http;
using System.IO;
using TransportApp.Models;
using Newtonsoft.Json;
using TransportApp.DbCore;



namespace TransportApp.Helpers
{
    public static class GoogleMatrixHelper
    {

        public const decimal ONFOOT = 0.28m;

        private static DistanceMatrix GetDistanceMatrix(List<Orders> origins, List<Orders> dests, bool fromStart, bool toStart)
        {
            var query = BuildApiQuery(origins, dests, fromStart, toStart);
            var matrix = GetMatrix(query);

            return matrix;
        }


        private static DistanceMatrix GetMatrix(string uri)
        {
            var client = new HttpClient();
            var stringTask = client.GetStringAsync(uri);
            var msg = stringTask;

            msg.Wait();

            var matrix = JsonConvert.DeserializeObject<DistanceMatrix>(msg.Result);
            return matrix;
        }



        public static void AddPointToMatrix(RepoContext context, Orders newModel)
        {
            var distances = new List<Distance>();
            var driveOffers = context.Orders.Where(x => x.Request == false)
                .Where(x=> x.Id != newModel.Id).OrderBy(x=> x.Id)
                .ToList();
             

            var orderIds = driveOffers.Select(x => x.Id).ToList();

            var self = GetDistanceMatrix(new List<Orders> { newModel }, new List<Orders> { newModel }, true, false);

            var firstrow = self.rows.First();
            int idDist = context.Distance.Max(x => x.Id) + 1;
            bool passenger = newModel.Request;

            //todo geographical_descriptions


            distances.Add(new Distance
            {
                Id = idDist++,
                Id_Start = newModel.Id,
                Id_End = newModel.Id,
                DistanceMeters = Int32.Parse(self.rows[0].elements[0].distance.value),
                Time = newModel.Request ? 0 : Int32.Parse(self.rows[0].elements[0].duration.value),
                OnFoot = passenger
            });
            
            newModel.StartDescription = self.origin_addresses.First();
            newModel.EndDescription = self.destination_addresses.First();
            


            if (driveOffers.Count > 0) //odleglosci miedzy pozostalymi pktami
            {
                    var myEndTheirStarts = GetDistanceMatrix(new List<Orders> { newModel }, driveOffers, passenger, true);

                        var row = myEndTheirStarts.rows.First().elements;

                        for (int j = 0; j < row.Count; j++)
                        {
                            distances.Add(new Distance
                            {
                                Id = idDist++,
                                Id_Start = newModel.Id,
                                Id_End = orderIds.ElementAt(j),
                                DistanceMeters = Int32.Parse(row.ElementAt(j).distance.value),
                                Time = 0 ,
                                OnFoot = true
                            });
                        }

                        
                    var theirEndsMyStarts = GetDistanceMatrix(driveOffers, new List<Orders> { newModel }, false, !passenger);
                    
                    for (int i = 0; i < theirEndsMyStarts.rows.Count; i++)
                    {
                        var currentRow = theirEndsMyStarts.rows[i];

                        distances.Add(new Distance
                        {
                            Id = idDist++,
                            Id_Start = orderIds.ElementAt(i),
                            Id_End = newModel.Id,
                            DistanceMeters = Int32.Parse(currentRow.elements.ElementAt(0).distance.value),
                            Time = 0,
                            OnFoot = true
                        });
                    }              
            } 
            


            if (passenger == false) //uzupelnienie od kierowcy do istniejacych pasazerow
            {
                
                var requests = context.Orders.Where(x => x.Request).OrderBy(x=> x.Id).ToList();

                if (requests.Count > 0)
                {
                    var theirStartsMyStart = GetDistanceMatrix(requests, new List<Orders> { newModel }, true, true);

                    for (int i = 0; i < theirStartsMyStart.rows.Count; i++)
                    {
                        var currentRow = theirStartsMyStart.rows[i];

                        distances.Add(new Distance
                        {
                            Id = idDist++,
                            Id_Start = requests.ElementAt(i).Id,
                            Id_End = newModel.Id,
                            DistanceMeters = Int32.Parse(currentRow.elements.ElementAt(0).distance.value),
                            Time = 0,
                            OnFoot = true
                        });
                    }

                    var myStartTheirEnds = GetDistanceMatrix(new List<Orders> { newModel }, requests, false, false);

                    var row = myStartTheirEnds.rows.First().elements;

                    for (int j = 0; j < row.Count; j++)
                    {
                        distances.Add(new Distance
                        {
                            Id = idDist++,
                            Id_Start = newModel.Id,
                            Id_End = requests.ElementAt(j).Id,
                            DistanceMeters = Int32.Parse(row.ElementAt(j).distance.value),
                            Time = 0,
                            OnFoot = true
                        });
                    }
                }
            }


            context.Distance.AddRange(distances);
            context.SaveChanges();
        }
        
        
        

        
        private static string BuildApiQuery(List<Orders> origins, List<Orders> destinations, bool fromStart, bool toStart)
        {

            var queryBuilder = new StringBuilder();

            queryBuilder.Append("https://maps.googleapis.com/maps/api/distancematrix/json?units=metrical&origins=");




            origins.ForEach(x => 
            {
                if (fromStart)
                {
                    queryBuilder.Append(x.StartLat.ToString().Replace(',', '.'));
                    queryBuilder.Append(",");
                    queryBuilder.Append(x.StartLng.ToString().Replace(',', '.'));
                    queryBuilder.Append("|");
                }
                else
                {
                    queryBuilder.Append(x.EndLat.ToString().Replace(',', '.'));
                    queryBuilder.Append(",");
                    queryBuilder.Append(x.EndLng.ToString().Replace(',', '.'));
                    queryBuilder.Append("|");
                }
            });

            queryBuilder.Append("&destinations=");

            destinations.ForEach(x =>
            {
                if (toStart) //todo mozna uwspolnic
                {
                    queryBuilder.Append(x.StartLat.ToString().Replace(',', '.'));
                    queryBuilder.Append(",");
                    queryBuilder.Append(x.StartLng.ToString().Replace(',', '.'));
                    queryBuilder.Append("|");
                }
                else
                {
                    queryBuilder.Append(x.EndLat.ToString().Replace(',', '.'));
                    queryBuilder.Append(",");
                    queryBuilder.Append(x.EndLng.ToString().Replace(',', '.'));
                    queryBuilder.Append("|");
                }
            });

            queryBuilder.Append("&key=AIzaSyALg39D5Tw7TbTYIy0GTjTeICHvKoGmrb4");
            return queryBuilder.ToString();
        }


    }
}
