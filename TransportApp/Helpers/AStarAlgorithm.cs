using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportApp.DbCore;
using TransportApp.Models;

namespace TransportApp.Helpers
{

    public class AStarAlgorithm
    {
        public const decimal DISTTOVEL = 1.38m;
        public const decimal DISTTOVELFAST = 12m;


        public Dictionary<int, Node> Nodes { get; set; }

        public List<Node> OpenNodes;
        public List<Node> ClosedNodes;

        public int StartNodeId { get; set; }
        public int EndNodeId { get; set; }

        public bool OneSwitchConstraint { get; set; }


        //jakos koszty odwiedzen itp

        public AStarAlgorithm(RepoContext context, AStarParameters parameters)
        {
            var startPoint = context.Orders.Where(x => x.Request).First();

            StartNodeId = startPoint.Id;
            OneSwitchConstraint = parameters.Switches == SwitchesEnum.Max1;


            //lista idkow przejazdow, ktore sa obliczone i powiazane z pktem startowym, zawiera rowniez pkt startowy
            var orderIdList = context.Distance.Where(x => x.Id_Start == startPoint.Id)
                .Select(x => x.Id_End).Distinct().ToList();

            var distances = context.Distance.ToList();
            decimal footCoeff = (2 - Decimal.Divide(parameters.FootPercentage, 100)); //czyli od 1 do 2, im wiecej tym gorzej chodzic pieszo



            //czasy pieszych
            foreach (var dist in distances.Where(x=> x.OnFoot))
            {
                dist.Time = (dist.DistanceMeters / DISTTOVEL) * footCoeff;
            }
            

            var carTimes = distances.Where(x => x.Id_Start == x.Id_End).ToDictionary(x => x.Id_Start, x => x.Time);

            //szacowana odleglosc dla konta z poszczegolnych aut - koncow podrozy
            var carToEndDictionary = distances.Where(x => x.Id_End == startPoint.Id) //tudu
                                        .ToDictionary(x => x.Id_Start, x => x.DistanceMeters / DISTTOVELFAST);






            Nodes = new Dictionary<int, Node>();

            EndNodeId = orderIdList.Max() + 1;



            //utworzenie wezlow grafu
            foreach (var orderId in orderIdList)
            {
                var node = new Node
                {
                    Id = orderId,
                    EstimatedWeightToEnd = carToEndDictionary[orderId],                    
                    NeighbourWeighDictionary = new Dictionary<int,decimal> ()
                };


                var distancesToNeighbour = distances.Where(x => x.Id_Start == orderId && x.Id_End != orderId).ToList();

                var closestDistance = distancesToNeighbour.Min(x => x.DistanceMeters / DISTTOVEL);
                node.EstimatedWeightToEnd += closestDistance;


                foreach (var dist in distancesToNeighbour)
                {
                    if (dist.Id_End == StartNodeId)
                    {
                        node.NeighbourWeighDictionary.Add(EndNodeId, dist.Time);
                        //bo pozostaje tylko podejsc pieszo do punktu, ta krawedz nie ma juz jazdy samochodem
                    }
                    else
                    {
                        var edgeWeigh = carTimes[dist.Id_End] + dist.Time;

                        if (parameters.Switches == SwitchesEnum.No && dist.Id_Start != StartNodeId)
                        {
                            edgeWeigh = 1000000000; //wartosc do stalych
                        }

                        node.NeighbourWeighDictionary.Add(dist.Id_End, edgeWeigh);
                    }
                                   
                }
                Nodes.Add(orderId, node);
            }

            Nodes.Add(EndNodeId, new Node
            {
                EstimatedWeightToEnd = 0,
                Id = EndNodeId,
                NeighbourWeighDictionary = new Dictionary<int, decimal> ()
            });
        }


        public List<int> CountAlgorithm()
        {
            OpenNodes = new List<Node> { Nodes[StartNodeId] };
            ClosedNodes = new List<Node>();

            var cameFrom = new Dictionary<int, int?>();

            foreach (var node in Nodes)
            {
                cameFrom.Add(node.Value.Id, null);
            }



            while (OpenNodes.Count > 0)
            {
                var nodesToPop = OpenNodes.OrderBy(x => (x.EstimatedWeightToEnd + x.CostFromStart)).ToList();

                if (OneSwitchConstraint)
                {
                    nodesToPop = nodesToPop.Where(x => MatchOneSwitchCondition(x)).ToList();
                }



                var poppedNode = nodesToPop.First();

                if (poppedNode.Id == EndNodeId)
                {
                    break;
                }

                OpenNodes.Remove(poppedNode);
                ClosedNodes.Add(poppedNode);

                foreach (var neighbour in poppedNode.NeighbourWeighDictionary)
                {
                    var succesor = Nodes[neighbour.Key];

                    decimal g = poppedNode.CostFromStart + neighbour.Value;
                    

                    if (OpenNodes.Contains(succesor))
                    {
                        if (succesor.CostFromStart > g)
                        {
                            cameFrom[succesor.Id] = poppedNode.Id;
                            succesor.ParentId = poppedNode.Id;
                            succesor.CostFromStart = g;
                        }
                        //else pomijany
                    }
                    else if (ClosedNodes.Contains(succesor))
                    {
                        if (succesor.CostFromStart > g)
                        {
                            ClosedNodes.Remove(succesor);
                            cameFrom[succesor.Id] = poppedNode.Id;
                            succesor.ParentId = poppedNode.Id;
                            succesor.CostFromStart = g;
                            OpenNodes.Add(succesor);
                        }
                        //else pomijany
                    }
                    else
                    {
                        cameFrom[succesor.Id] = poppedNode.Id;
                        succesor.ParentId = poppedNode.Id;
                        succesor.CostFromStart = g;
                        OpenNodes.Add(succesor);
                    }
                 
                }                
            }

            var used = new List<int>();
            int? next = cameFrom[EndNodeId];

            while (next.HasValue  && next.Value != StartNodeId)
            {
                used.Add(next.Value);
                next = cameFrom[next.Value];
            }


            return used;
        }

        private bool MatchOneSwitchCondition(Node node)
        {
            var result = true;

            if (node.Id != StartNodeId && node.Id != EndNodeId && node.ParentId != StartNodeId && node.ParentId != 0)
            {
                var parent = Nodes[node.ParentId];

                result = parent.ParentId == StartNodeId || parent.ParentId == 0;
            }

            return result;
        }

    }



    public class Node
    {
        public int Id;

        public int ParentId;


        public decimal CostFromStart;

        public decimal? CheapestCostOfReaching;

        public decimal EstimatedWeightToEnd;


        public Dictionary<int, decimal> NeighbourWeighDictionary;
        
    }



}
