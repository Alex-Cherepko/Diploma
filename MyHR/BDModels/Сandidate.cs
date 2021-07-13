using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHR
{
    public class Сandidate
    {
        public int СandidateId { get; set; }

        public string FullName { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string Description { get; set; }

        public int VacancyId { get; set; }

        public Vacancy Vacancy { get; set; }
    }
}
