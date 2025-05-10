using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropagacjaWstecznaJT.Supporting_Classes
{
    class TrainingSample
    {
        public double[] Inputs { get; set; }
        public double[] ExpectedOutputs { get; set; }

        public TrainingSample(double[] inputs, double[] outputs)
        {
            Inputs = inputs;
            ExpectedOutputs = outputs;
        }
    }
}
