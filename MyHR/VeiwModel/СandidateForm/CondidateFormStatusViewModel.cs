using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHR
{
    public class CondidateFormStatusViewModel : BaseViewModel
    {
        public CondidateFormStatus CondidateFormStatus { get; set; }
        public List<CondidateFormStatus> CondidateFormStatusList = new List<CondidateFormStatus>()
        {
            new CondidateFormStatus(1,"В работе"),
            new CondidateFormStatus(2,"Прием"),
            new CondidateFormStatus(3,"Отказ")
        };

        internal CondidateFormStatus GetByName(string status)
        {
            if(!string.IsNullOrEmpty(status))
            return CondidateFormStatusList.Find(item=> item.Name.Contains(status));

            return null;
        }
    }

    public class CondidateFormStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CondidateFormStatus(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
