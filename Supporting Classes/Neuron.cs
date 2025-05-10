using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropagacjaWstecznaJT.Supporting_Classes
{
    class Neuron
    {
        public List<double> Weights { get; set; }
        public double Bias { get; set; }
        public double Output { get; private set; }
        public double Delta { get; set; }

        private static readonly Random rand = new Random();

        public Neuron(int inputCount)
        {
            Weights = new List<double>();
            for (int i = 0; i < inputCount; i++)
                Weights.Add(rand.NextDouble() * 10 - 5); // Losuj z przedziału [-5;5]
            Bias = rand.NextDouble() * 10 - 5;
        }

        public double Activate(List<double> inputs, double beta)
        {
            double sum = Bias;
            for (int i = 0; i < inputs.Count; i++)
                sum += inputs[i] * Weights[i];
            Output = 1.0 / (1.0 + Math.Exp(-beta * sum)); // Funkcja sigmoidalna
            return Output;
        }

        public double Derivative(double beta)
        {
            return beta * Output * (1.0 - Output); // Pochodna sigmoidy
        }
    }
}
