using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHR
{
    public class CondidateStatusViewModel : BaseViewModel
    {
        public CondidateStatus CondidateStatus { get; set; }
        public List<CondidateStatus> CondidateStatusList = new List<CondidateStatus>()
        {
            new CondidateStatus(1,"В работе"),
            new CondidateStatus(2,"Прием"),
            new CondidateStatus(3,"Резерв"),
            new CondidateStatus(4,"Отказ"),
            new CondidateStatus(5,"Черный список")

        };

        internal CondidateStatus GetByName(string status)
        {
            if(!string.IsNullOrEmpty(status))
            return CondidateStatusList.Find(item => item.Name.Contains(status));

            return null;
        }
    }

    public class CondidateStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CondidateStatus(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
