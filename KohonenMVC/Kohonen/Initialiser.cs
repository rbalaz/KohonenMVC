using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;

namespace KohonenMVC.Kohonen
{
    class Initialiser
    {
        private int numberOfInputs;
        private double learningParameter;
        private double adaptiveHeight;
        private int layerWidth;
        private int layerLength;
        private Random random;
        private string fileName;
        public int imageWidth { get; set; }
        public int imageHeight { get; set; }
        public Data[] trainingData { get; private set; }

        public Initialiser(int numberOfInputs, double learningParameter, double adaptiveHeight, int width, int length)
        {
            this.learningParameter = learningParameter;
            this.adaptiveHeight = adaptiveHeight;
            this.numberOfInputs = numberOfInputs;
            layerWidth = width;
            layerLength = length;
            random = new Random();
        }

        public Initialiser(string fileName)
        {
            this.fileName = fileName;
        }

        public Network createKohonenNetwork()
        {
            List<Neuron> kohonenLayer = new List<Neuron>();

            for (int i = 0; i < layerLength; i++)
            {
                for (int j = 0; j < layerWidth; j++)
                {
                    Neuron neuron = new Neuron(i, j);
                    List<Synapse> synapses = new List<Synapse>();
                    for (int k = 0; k < numberOfInputs; k++)
                        synapses.Add(new Synapse());
                    neuron.synapses = synapses;
                    kohonenLayer.Add(neuron);
                }
            }

            Network network = new Network(kohonenLayer, learningParameter, adaptiveHeight, layerWidth, layerLength);
            return network;
        }

        public void initialiseKohonenNetwork(Network kohonenNetwork, Data[] trainingData)
        {
            for (int i = 0; i < kohonenNetwork.layerLength; i++)
            {
                for (int j = 0; j < kohonenNetwork.layerWidth; j++)
                {
                    int pattern = (random.Next(0, trainingData.Length - 1));
                    Neuron n = kohonenNetwork.kohonenLayer.Find(neuron => neuron.layerPosition.x == i && neuron.layerPosition.y == j);
                    foreach (Synapse s in n.synapses)
                        s.weight = trainingData[pattern].data[n.synapses.IndexOf(s)];
                }
            }
        }

        public Network loadKohonenNetwork()
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberDecimalSeparator = ".";

            int layerWidth, layerLength;
            string line = reader.ReadLine();
            string[] lineSegments = line.Split(' ');
            layerWidth = int.Parse(lineSegments[0]);
            layerLength = int.Parse(lineSegments[1]);
            double learningParameter = double.Parse(reader.ReadLine());
            double adaptiveHeight = double.Parse(reader.ReadLine());
            string[] imageParameters = reader.ReadLine().Split(' ');
            imageWidth = int.Parse(imageParameters[0]);
            imageHeight = int.Parse(imageParameters[1]);

            List<Neuron> kohonenLayer = new List<Neuron>();
            int rowCount = -1, colCount = -1;
            while ((line = reader.ReadLine()) != "Training data")
            {
                if (line.Contains("Row"))
                {
                    rowCount++;
                    colCount = -1;
                    continue;
                }
                if (line.Contains("Neuron"))
                {
                    colCount++;
                    continue;
                }
                Neuron neuron = new Neuron(rowCount, colCount);
                line = line.Trim();
                string[] stringSynapses = line.Split(' ');
                List<Synapse> synapses = new List<Synapse>();
                for (int i = 0; i < stringSynapses.Length; i++)
                {
                    Synapse s = new Synapse();
                    s.weight = double.Parse(stringSynapses[i]);
                    synapses.Add(s);
                }
                neuron.synapses = synapses;
                kohonenLayer.Add(neuron);
            }

            List<Data> data = new List<Data>();
            while ((line = reader.ReadLine()) != null)
            {
                int[] dataPieces = (line.Trim().Split(' ')).Select(s => int.Parse(s)).ToArray();
                data.Add(new Data(dataPieces));
            }

            trainingData = data.ToArray();
            Network network = new Network(kohonenLayer, learningParameter, adaptiveHeight, layerWidth, layerLength);
            return network;
        }
    }
}
