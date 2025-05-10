using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropagacjaWstecznaJT.Supporting_Classes
{
    class Layer
    {
        public List<Neuron> Neurons { get; private set; }

        public Layer(int neuronCount, int inputCount)
        {
            Neurons = new List<Neuron>();
            for (int i = 0; i < neuronCount; i++)
                Neurons.Add(new Neuron(inputCount));
        }

        public List<double> Activate(List<double> inputs, double beta)
        {
            return Neurons.Select(n => n.Activate(inputs, beta)).ToList();
        }
    }
}
