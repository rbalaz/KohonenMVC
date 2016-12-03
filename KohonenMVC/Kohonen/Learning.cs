using System;
using System.IO;

namespace KohonenMVC.Kohonen
{
    public class Learning
    {
        private Network network;
        private double preSetLearningParameter;
        private int numberOfIterations;
        private Data[] trainingData;
        private double preSetAdaptiveHeight;
        private int[] imageDimension;
        public double[] maximumWeightChanges { get; private set; }
        public string folderPath { get; set; }

        public Learning(Network network,  Data[] trainingData, int numberOfIterations, int[] imageDimension)
        {
            this.network = network;
            this.trainingData = trainingData;
            this.numberOfIterations = numberOfIterations;
            this.imageDimension = imageDimension;
            preSetLearningParameter = network.learningParameter;
            preSetAdaptiveHeight = network.adaptiveHeight;
        }

        public void executeTrainingCycle()
        {
            double neighbourhoodRadius;
            double adjustedNumberOfIterations = numberOfIterations / 3.0;
            double decayFunction;
            double higherDimension = network.layerLength > network.layerWidth ? network.layerLength : network.layerWidth / 2.0;
            double timeVariable = higherDimension > 1 ? adjustedNumberOfIterations / Math.Log(higherDimension) : adjustedNumberOfIterations / 2;
            double globalMaximumWeightChange;
            maximumWeightChanges = new double[numberOfIterations];
            for (int i = 1; i <= numberOfIterations; i++)
            {
                globalMaximumWeightChange = 0;
                decayFunction = higherDimension * Math.Exp(-i / timeVariable);
                neighbourhoodRadius = -2 * Math.Pow(decayFunction, 2);
                for (int j = 0; j < trainingData.Length; j++)
                {
                    network.findLoserAndChangeWeights(trainingData[j], neighbourhoodRadius);
                    if (globalMaximumWeightChange < network.maximumWeightChange)
                        globalMaximumWeightChange = network.maximumWeightChange;
                }
                maximumWeightChanges[i - 1] = globalMaximumWeightChange;
                network.learningParameter = adjustLearningParameter(preSetLearningParameter, i, adjustedNumberOfIterations);
                network.adaptiveHeight = preSetAdaptiveHeight * Math.Exp((double)-i / adjustedNumberOfIterations);
            }

            saveNeuralNetwork();
        }

        private void saveNeuralNetwork()
        {
            string fileName = (new Random()).Next(0, 987654).ToString() + ".txt";
            string filePath = Path.Combine(folderPath, fileName);
            FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine(network.layerWidth + " " + network.layerLength);
            writer.WriteLine(network.learningParameter);
            writer.WriteLine(network.adaptiveHeight);
            writer.WriteLine(imageDimension[0] + " " + imageDimension[1]);
            for (int i = 0; i < network.layerLength; i++)
            {
                writer.WriteLine("Row" + (i + 1));
                for (int j = 0; j < network.layerWidth; j++)
                {
                    writer.WriteLine("Neuron" + (j + 1));
                    Neuron neuron = network.kohonenLayer.Find(n => n.layerPosition.x == i && n.layerPosition.y == j);
                    foreach (Synapse s in neuron.synapses)
                        writer.Write(s.weight + " ");
                    writer.WriteLine();
                }
            }

            writer.WriteLine("Training data");
            for (int i = 0; i < trainingData.Length; i++)
            {
                writer.WriteLine(trainingData[i]);
            }

            writer.Close();
            stream.Close();
        }

        private double adjustLearningParameter(double learningParameter, int iterationCounter, double adjustedNumberOfIterations)
        {
            return learningParameter * Math.Exp((double)-iterationCounter / adjustedNumberOfIterations);
        }
    }
}
