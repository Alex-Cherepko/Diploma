using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyHR
{
    /// <summary>
    /// Логика взаимодействия для NewOrderPage.xaml
    /// </summary>
    public partial class NewOrderPage : UserControl
    {
        public NewOrderPage()
        {
            InitializeComponent();
        }
        public NewOrderPage(PropertyChangeModel propertyChangeModel, ApplicationPageCommands applicationPageCommands, Order CurrPosition = null) :this()
        {
            DataContext = new NewOrderPageViewModel(propertyChangeModel, applicationPageCommands, CurrPosition);
        }
    }
}
