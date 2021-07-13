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
    /// Логика взаимодействия для NewСandidatePage.xaml
    /// </summary>
    public partial class NewСandidatePage : UserControl
    {
        public NewСandidatePage()
        {
            InitializeComponent();
        }
        public NewСandidatePage(PropertyChangeModel propertyChangeModel, ApplicationPageCommands applicationPageCommands, Сandidate CurrPosition = null) :this()
        {
            DataContext = new NewСandidatePageViewModel(propertyChangeModel, applicationPageCommands, CurrPosition);
        }
    }
}
