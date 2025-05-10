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
using PropagacjaWstecznaJT.UserControls;

namespace PropagacjaWstecznaJT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentFrame.Content = new Task1();
        }
        private void ShowTask1(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Task1();
        }
        private void ShowTask2(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Task2();
        }
        private void ShowTask3(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Task3();
        }
    }
}