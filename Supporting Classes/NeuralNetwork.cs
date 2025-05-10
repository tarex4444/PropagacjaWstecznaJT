using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropagacjaWstecznaJT.Supporting_Classes
{
    class NeuralNetwork
    {
        private List<Layer> Layers;
        private double Beta;
        private double Mu;

        public NeuralNetwork(int[] structure, double beta = 1.0, double mu = 0.3)
        {
            Beta = beta;
            Mu = mu;
            Layers = new List<Layer>();
            for (int i = 1; i < structure.Length; i++)
                Layers.Add(new Layer(structure[i], structure[i - 1]));
        }

        public double[] Run(double[] inputs)
        {
            List<double> output = inputs.ToList();
            foreach (var layer in Layers)
                output = layer.Activate(output, Beta);
            return output.ToArray();
        }

        public void Train(List<TrainingSample> samples, int epochs)
        {
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                foreach (var sample in samples)
                    TrainSample(sample);
            }
        }

        private void TrainSample(TrainingSample sample)
        {
            var outputs = new List<List<double>> { sample.Inputs.ToList() };
            foreach (var layer in Layers)
                outputs.Add(layer.Activate(outputs.Last(), Beta));

            // Oblicz delty dla warstwy wyjściowej
            for (int i = Layers.Last().Neurons.Count - 1; i >= 0; i--)
            {
                var neuron = Layers.Last().Neurons[i];
                double error = sample.ExpectedOutputs[i] - neuron.Output;
                neuron.Delta = error * neuron.Derivative(Beta);
            }

            // Oblicz delty dla warstw ukrytych
            for (int l = Layers.Count - 2; l >= 0; l--)
            {
                for (int i = 0; i < Layers[l].Neurons.Count; i++)
                {
                    double sum = 0;
                    foreach (var neuron in Layers[l + 1].Neurons)
                        sum += neuron.Weights[i] * neuron.Delta;
                    Layers[l].Neurons[i].Delta = sum * Layers[l].Neurons[i].Derivative(Beta);
                }
            }

            // Aktualizuj wagi i biasy
            for (int l = 0; l < Layers.Count; l++)
            {
                var inputs = outputs[l];
                foreach (var neuron in Layers[l].Neurons)
                {
                    for (int w = 0; w < neuron.Weights.Count; w++)
                        neuron.Weights[w] += Mu * neuron.Delta * inputs[w];
                    neuron.Bias += Mu * neuron.Delta;
                }
            }
        }
    }
}
