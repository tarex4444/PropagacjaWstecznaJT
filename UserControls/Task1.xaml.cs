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
using PropagacjaWstecznaJT.Supporting_Classes;

namespace PropagacjaWstecznaJT.UserControls
{
    /// <summary>
    /// Logika interakcji dla klasy Task1.xaml
    /// </summary>
    public partial class Task1 : UserControl
    {
        private NeuralNetwork xorNetwork;

        public Task1()
        {
            InitializeComponent();

            xorNetwork = new NeuralNetwork(new[] { 2, 2, 1 }, beta: 1.0, mu: 0.3);

            var trainingData = new List<TrainingSample>
            {
                new TrainingSample(new[] { 0.0, 0.0 }, new[] { 0.0 }),
                new TrainingSample(new[] { 0.0, 1.0 }, new[] { 1.0 }),
                new TrainingSample(new[] { 1.0, 0.0 }, new[] { 1.0 }),
                new TrainingSample(new[] { 1.0, 1.0 }, new[] { 0.0 }),
            };

            xorNetwork.Train(trainingData, 50000);
        }

        private void RunNetwork(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Input1.Text, out double x1) &&
                double.TryParse(Input2.Text, out double x2))
            {
                var result = xorNetwork.Run(new[] { x1, x2 });
                OutputText.Text = $"Wynik: {result[0]:F3}";
            }
            else
            {
                OutputText.Text = "Błędne dane wejściowe.";
            }
        }
    }
}
