using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportApp.Models
{
    public class Vehicle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string CarModel { get; set; }
        public string CarNumber { get; set; }
        public int? Year { get; set; }

    }
}
