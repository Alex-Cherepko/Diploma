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
    /// Логика взаимодействия для OpenPageItemControl.xaml
    /// </summary>
    public partial class OpenPageItemControl : UserControl
    {
        public OpenPageItemControl()
        {
            InitializeComponent();
        }
        public OpenPageItemControl(string NamePage, UserControl CurrentPage, UserControl NextPage, PropertyChangeModel PropertyChangeModel) :this()
        {
            DataContext = new OpenPageItemViewModel(NamePage, CurrentPage, NextPage, PropertyChangeModel);

        }

        
    }
}
