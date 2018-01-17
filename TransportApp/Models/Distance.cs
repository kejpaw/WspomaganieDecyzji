using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportApp.Models
{
    public class Distance
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Id_Start { get; set; }

        public int Id_End { get; set; }

        public int DistanceMeters { get; set; }
        public decimal Time { get; set; }

        public bool OnFoot { get; set; }

    }
}
