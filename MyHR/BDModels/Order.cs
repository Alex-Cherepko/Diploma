using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHR
{
    public class Order
    {
        public int OrderId { get; set; }

        public int Code { get; set; }

        public DateTime DocDate { get; set; }

        public DateTime ExecutionTerm { get; set; }

        public string Status { get; set; }

    }
}
