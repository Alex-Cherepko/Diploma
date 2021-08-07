using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHR
{
    public class OrderStatusViewModel : BaseViewModel
    {
        public OrderStatus OrderStatus { get; set; }
        public List<OrderStatus> OrderStatusList = new List<OrderStatus>()
        {
            new OrderStatus(1,"План"),
            new OrderStatus(2,"В работе"),
            new OrderStatus(3,"Закрыта")
        };

        internal OrderStatus GetByName(string status)
        {
            if (!string.IsNullOrEmpty(status))
            return OrderStatusList.Find(item => item.Name.Contains(status));

            return null;
        }

    }

    public class OrderStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public OrderStatus(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
