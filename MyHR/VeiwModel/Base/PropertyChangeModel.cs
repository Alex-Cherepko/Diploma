using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyHR
{
    public class PropertyChangeModel : IMessageBus
    {
        public event ValueChangedHandler ValueChanged;
        public event ClosePageHandler ClosePageIvent;
        public event ChagePageHandler ChagePageIvent;
        public event SendValueHandler SendValueEvent;

        public void ChagePage(UserControl CurrPage)
        {
            ChagePageIvent?.Invoke(this, CurrPage);
        }

        public void ClosePage(UserControl CurrPage)
        {
            ClosePageIvent?.Invoke(this, CurrPage);
        }

        public void SendValue(object valueName, object newValue, object senderedValue)
        {
            ValueChanged?.Invoke(this, valueName, newValue, senderedValue);
        }

        public void SendValueToOwner(object Value)
        {
            SendValueEvent?.Invoke(this, Value);
        }
    }
}
