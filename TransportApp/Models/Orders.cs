using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportApp.Models
{
    public class Orders /*: BaseModel*/
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public decimal StartLat { get; set; }
        public decimal StartLng { get; set; }

        public decimal EndLat { get; set; }
        public decimal EndLng { get; set; }

        public int? IdUser { get; set; }
        public int? IdDriver { get; set; }

        public bool IsInProgress { get; set; }
        public  DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool Request { get; set; }

        public string StartDescription { get; set; }
        public string EndDescription { get; set; }


    }
}
