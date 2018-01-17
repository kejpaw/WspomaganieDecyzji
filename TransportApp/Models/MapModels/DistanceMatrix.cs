using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportApp.Models
{
    public class DistanceMatrix
    {

        public List<string> destination_addresses { get; set; }
        public List<string> origin_addresses { get; set; }

        public List<Row> rows { get; set; }

        public string status { get; set; }

    }

    public class Row
    {
        public List<Element> elements { get; set; }

    }

    public class Element
    {

        public Node distance { get; set; }

        public Node duration { get; set; }
        public string status { get; set; }

    }


    public class Node
    {
        public string text { get; set; }
        public string value { get; set; } //ostrożnie!

    }

}
