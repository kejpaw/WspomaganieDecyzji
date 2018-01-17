using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportApp.Models
{
    public class TestTable
    {
        //todo basemodel

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TestNumber { get; set; }
        public DateTime? TestDate { get; set; }
        public bool TestBool { get; set; }

    }
}
