using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHR
{
    public class СandidateForm
    {
        public int СandidateFormId { get; set; }

        public int Code { get; set; }

        public DateTime DocDate { get; set; }

        public string Sity { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public int СandidateId { get; set; }

        public virtual Сandidate Сandidate { get; set; }

       public int VacancyId { get; set; }

        public virtual Vacancy Vacancy { get; set; }
    }
}
