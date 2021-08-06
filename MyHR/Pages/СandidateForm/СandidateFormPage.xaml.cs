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
    /// Логика взаимодействия для PositionPage.xaml
    /// </summary>
    public partial class СandidateFormPage : UserControl
    {
        public СandidateFormPage()
        {
            InitializeComponent();
        }

        public СandidateFormPage(PropertyChangeModel propertyChangeModel):this()
        {
            DataContext = new СandidateFormPageViewModel(propertyChangeModel);
        }
    }
}
