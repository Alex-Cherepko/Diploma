using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyHR
{
    public delegate void ValueChangedHandler(object sender, object valueName, object newValue, object senderedValue);

    public delegate void ClosePageHandler(object sender, UserControl CurrPage);

    public delegate void ChagePageHandler(object sender, UserControl CurrPage);

    public delegate void SendValueHandler(object sender, object Value);
    public interface IMessageBus
    {
        event ValueChangedHandler ValueChanged;
        event ClosePageHandler ClosePageIvent;
        event ChagePageHandler ChagePageIvent;
        event SendValueHandler SendValueEvent;

        void SendValue(object valueName, object newValue, object senderedValue);
        void ClosePage(UserControl CurrPage);
        void ChagePage(UserControl CurrPage);
        void SendValueToOwner(object Value);

    }
}
