using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHR
{
    public class Orders_CandidateForm
    {
        public int Orders_CandidateFormId { get; set; }

        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int СandidateFormId { get; set; }

        public virtual СandidateForm CandidateForm { get; set; }

    }
}
