using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHR
{
    public class Order_Vacancy
    {
        public int Order_VacancyId { get; set; }

        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int VacancyId { get; set; }

        public virtual Vacancy Vacancy { get; set; }


    }
}
