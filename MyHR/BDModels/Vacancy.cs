using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHR
{
    public class Vacancy
    {
        public int VacancyId { get; set; }

        public int Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PositionId { get; set; }

        public virtual Position Position { get; set; }
    }
}
