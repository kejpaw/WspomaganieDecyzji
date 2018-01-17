using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportApp.Models
{
    public class Driver
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int? IdVehicle { get; set; }

        public int Region { get; set; }

    }
}
