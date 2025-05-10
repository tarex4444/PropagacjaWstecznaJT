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
    /// Logika interakcji dla klasy Task2.xaml
    /// </summary>
    public partial class Task2 : UserControl
    {
        private NeuralNetwork network;

        public Task2()
        {
            InitializeComponent();

            network = new NeuralNetwork(new[] { 2, 2, 2, 2 }, beta: 1.0, mu: 0.3);

            var trainingData = new List<TrainingSample>
            {
                new TrainingSample(new[] { 0.0, 0.0 }, new[] { 0.0, 1.0 }),
                new TrainingSample(new[] { 0.0, 1.0 }, new[] { 1.0, 0.0 }),
                new TrainingSample(new[] { 1.0, 0.0 }, new[] { 1.0, 0.0 }),
                new TrainingSample(new[] { 1.0, 1.0 }, new[] { 0.0, 0.0 }),
            };

            network.Train(trainingData, 50000);
        }

        private void RunNetwork(object sender, RoutedEventArgs e)
        {
            double x1 = Input1Checkbox.IsChecked == true ? 1.0 : 0.0;
            double x2 = Input2Checkbox.IsChecked == true ? 1.0 : 0.0;

            var result = network.Run(new[] { x1, x2 });
            OutputText.Text = $"Wynik 1: {result[0]:F3} | Wynik 2: {result[1]:F3}";

            DrawNetwork(new[] { x1, x2 }, result);
        }

        private void DrawNetwork(double[] inputs, double[] outputs)
        {
            NetworkCanvas.Children.Clear();

            double neuronRadius = 30;
            double layerSpacing = 160;
            double verticalSpacing = 90;
            double marginLeft = 60;

            var layers = network.GetLayers();
            var layerPositions = new List<List<Point>>();
            var layerOutputs = new List<List<double>>();
            layerOutputs.Add(inputs.ToList());

            // Wylicz aktywacje
            List<double> current = inputs.ToList();
            foreach (var layer in layers)
            {
                current = layer.Activate(current, 1.0);
                layerOutputs.Add(current.ToList());
            }

            for (int l = 0; l < layerOutputs.Count; l++)
            {
                var positions = new List<Point>();
                int count = layerOutputs[l].Count;
                double offsetY = (NetworkCanvas.ActualHeight - count * verticalSpacing) / 2;

                for (int n = 0; n < count; n++)
                {
                    double x = marginLeft + l * layerSpacing;
                    double y = offsetY + n * verticalSpacing;
                    positions.Add(new Point(x, y));

                    // neuron
                    Ellipse neuron = new Ellipse
                    {
                        Width = neuronRadius * 2,
                        Height = neuronRadius * 2,
                        Stroke = Brushes.Black,
                        Fill = Brushes.White
                    };
                    Canvas.SetLeft(neuron, x - neuronRadius);
                    Canvas.SetTop(neuron, y - neuronRadius);
                    NetworkCanvas.Children.Add(neuron);

                    // bias lub input
                    string text = l == 0 ? $"{layerOutputs[l][n]:F2}" : $"{layers[l - 1].Neurons[n].Bias:F2}";
                    TextBlock tb = new TextBlock
                    {
                        Text = text,
                        Foreground = Brushes.Black,
                        FontSize = 12
                    };
                    Canvas.SetLeft(tb, x - 12);
                    Canvas.SetTop(tb, y - 8);
                    NetworkCanvas.Children.Add(tb);
                }

                layerPositions.Add(positions);
            }

            for (int l = 1; l < layerPositions.Count; l++)
            {
                var fromPoints = layerPositions[l - 1];
                var toPoints = layerPositions[l];
                var layer = layers[l - 1];

                for (int j = 0; j < toPoints.Count; j++)
                {
                    for (int i = 0; i < fromPoints.Count; i++)
                    {
                        Point p1 = fromPoints[i];
                        Point p2 = toPoints[j];

                        double weight = layer.Neurons[j].Weights[i];
                        double inputVal = layerOutputs[l - 1][i];

                        Line line = new Line
                        {
                            X1 = p1.X + neuronRadius,
                            Y1 = p1.Y,
                            X2 = p2.X - neuronRadius,
                            Y2 = p2.Y,
                            Stroke = weight >= 0 ? Brushes.Green : Brushes.Red,
                            StrokeThickness = 1 + Math.Min(3, Math.Abs(weight))
                        };
                        NetworkCanvas.Children.Add(line);

                        double midX = (line.X1 + line.X2) / 2;
                        double midY = (line.Y1 + line.Y2) / 2;

                        Line tick = new Line
                        {
                            X1 = midX - 5,
                            Y1 = midY - 5,
                            X2 = midX + 5,
                            Y2 = midY + 5,
                            Stroke = Brushes.Black,
                            StrokeThickness = 1
                        };
                        NetworkCanvas.Children.Add(tick);

                        TextBlock weightText = new TextBlock
                        {
                            Text = $"{weight:F2}",
                            FontSize = 10,
                            Foreground = Brushes.Black
                        };
                        Canvas.SetLeft(weightText, midX + 6);
                        Canvas.SetTop(weightText, midY);
                        NetworkCanvas.Children.Add(weightText);

                        TextBlock inputValText = new TextBlock
                        {
                            Text = $"{inputVal:F2}",
                            FontSize = 10,
                            Foreground = Brushes.Gray
                        };
                        Canvas.SetLeft(inputValText, (p1.X + p2.X) / 2);
                        Canvas.SetTop(inputValText, (p1.Y + p2.Y) / 2 + 10);
                        NetworkCanvas.Children.Add(inputValText);
                    }
                }
            }
        }
    }
}
