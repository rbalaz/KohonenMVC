using System;
using System.Collections.Generic;
using System.Linq;

namespace KohonenMVC.Kohonen
{
    public class Network
    {
        public List<Neuron> kohonenLayer { get; private set; }
        public int layerWidth { get; set; }
        public int layerLength { get; set; }
        public double learningParameter { get; set; }
        public double adaptiveHeight { get; set; }
        public double maximumWeightChange { get; set; }

        public Network(List<Neuron> kohonenLayer, double learningParameter, double adaptiveHeight, int width, int length)
        {
            this.kohonenLayer = kohonenLayer;
            this.learningParameter = learningParameter;
            this.adaptiveHeight = adaptiveHeight;
            layerWidth = width;
            layerLength = length;
            maximumWeightChange = 0;
        }

        public void findLoserAndChangeWeights(Data data, double neighbourhoodRadius)
        {
            maximumWeightChange = 0;
            foreach (Neuron n in kohonenLayer)
                n.evaluateOutput(data);

            Neuron loser = kohonenLayer.Find(neuron => neuron.output == kohonenLayer.Min(min => min.output));
            Coordinate loserPosition = loser.layerPosition;

            foreach (Neuron n in kohonenLayer)
            {
                double neighbourhoodFunction = adaptiveHeight * Math.Exp(Math.Pow(Coordinate.getEuclidDistance
                        (n.layerPosition, loserPosition), 2) / neighbourhoodRadius);
                foreach (Synapse s in n.synapses)
                {

                    double weightChange = learningParameter * neighbourhoodFunction * (data.data[n.synapses.IndexOf(s)] - s.weight);
                    if (maximumWeightChange == 0)
                        maximumWeightChange = weightChange;
                    else if (maximumWeightChange < weightChange)
                        maximumWeightChange = weightChange;
                    s.weight += weightChange;
                }
            }
        }
    }
}
