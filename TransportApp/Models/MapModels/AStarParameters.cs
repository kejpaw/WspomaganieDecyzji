using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportApp.Models
{

    public enum SwitchesEnum
    {
         No = 0, Max1 = 1, NoLimit = 2
    }


    public class AStarParameters
    {
        public int FootPercentage { get; set; }

        public SwitchesEnum Switches { get; set; }
    }
}
