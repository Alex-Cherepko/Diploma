using System.Windows;

namespace MyHR
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PropertyChangeModel modelPropertyChange = new PropertyChangeModel();
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new WindowViewModel(this, modelPropertyChange);
        }
    }
}
