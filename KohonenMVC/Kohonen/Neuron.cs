using System;
using System.Collections.Generic;

namespace KohonenMVC.Kohonen
{
    public class Neuron
    {
        public List<Synapse> synapses;
        public Coordinate layerPosition;
        public double output;

        public Neuron(int x, int y)
        {
            layerPosition = new Coordinate(x, y);
        }

        public void evaluateOutput(Data data)
        {
            output = 0;
            for (int i = 0; i < data.data.Count; i++)
            {
                output += Math.Pow(synapses[i].weight - data.data[i], 2);
            }
        }
    }
}
