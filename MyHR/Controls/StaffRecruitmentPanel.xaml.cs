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
    /// Логика взаимодействия для StaffRecruitmentPanel.xaml
    /// </summary>
    public partial class StaffRecruitmentPanel : UserControl
    {
        public StaffRecruitmentPanel()
        {
            InitializeComponent();
        }
        public StaffRecruitmentPanel(PropertyChangeModel propertyChangeModel, ApplicationUserControl applicationUserControl) : this()
        {
            DataContext = new UserControlViewModel(propertyChangeModel, applicationUserControl);
        }
    }
}
