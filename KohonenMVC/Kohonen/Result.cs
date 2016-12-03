using System.Linq;
using System.Drawing;

namespace KohonenMVC.Kohonen
{
    public class Result
    {
        private string filePath;
        private Network network;

        public Result(string filePath, Network network)
        {
            this.filePath = filePath;
            this.network = network;
        }

        public Bitmap createAndSaveResult()
        {
            Bitmap image;
            Graphics imageGraphics;
            if (filePath.Contains("pictureA"))
            {
                image = new Bitmap(filePath);
                Bitmap tempImage = new Bitmap(image.Width, image.Height);
                imageGraphics = Graphics.FromImage(tempImage);
                imageGraphics.DrawImage(image, 0, 0);
                image = tempImage;
            }
            else
            {
                image = new Bitmap(filePath);
                imageGraphics = Graphics.FromImage(image);
            }

            for (int i = 0; i < network.layerLength; i++)
            {
                for (int j = 0; j < network.layerWidth; j++)
                {
                    if (i > 0)
                    {
                        Neuron startNeuron = network.kohonenLayer.Find(n => n.layerPosition.x == i && n.layerPosition.y == j);
                        Neuron endNeuron = network.kohonenLayer.Find(n => n.layerPosition.x == (i - 1) && n.layerPosition.y == j);
                        drawLine(startNeuron, endNeuron, imageGraphics);
                    }
                    if (i < network.layerLength - 1)
                    {
                        Neuron startNeuron = network.kohonenLayer.Find(n => n.layerPosition.x == i && n.layerPosition.y == j);
                        Neuron endNeuron = network.kohonenLayer.Find(n => n.layerPosition.x == (i + 1) && n.layerPosition.y == j);
                        drawLine(startNeuron, endNeuron, imageGraphics);
                    }
                    if (j > 0)
                    {
                        Neuron startNeuron = network.kohonenLayer.Find(n => n.layerPosition.x == i && n.layerPosition.y == j);
                        Neuron endNeuron = network.kohonenLayer.Find(n => n.layerPosition.x == i && n.layerPosition.y == (j - 1));
                        drawLine(startNeuron, endNeuron, imageGraphics);
                    }
                    if (j < network.layerWidth - 1)
                    {
                        Neuron startNeuron = network.kohonenLayer.Find(n => n.layerPosition.x == i && n.layerPosition.y == j);
                        Neuron endNeuron = network.kohonenLayer.Find(n => n.layerPosition.x == i && n.layerPosition.y == (j + 1));
                        drawLine(startNeuron, endNeuron, imageGraphics);
                    }
                }
            }

            for (int i = 0; i < network.layerLength; i++)
                for (int j = 0; j < network.layerWidth; j++)
                    drawEllipse(network.kohonenLayer.Find(n => n.layerPosition.x == i && n.layerPosition.y == j), imageGraphics);

            return image;
        }

        private void drawLine(Neuron startNeuron, Neuron endNeuron, Graphics imageGraphics)
        {
            RealCoordinate start = new RealCoordinate(startNeuron.synapses.Select(synapse => synapse.weight).ToArray());
            RealCoordinate end = new RealCoordinate(endNeuron.synapses.Select(synapse => synapse.weight).ToArray());
            Pen linePen = new Pen(Color.Red, 2);
            imageGraphics.DrawLine(linePen, (float)start.x, (float)start.y, (float)end.x, (float)end.y);
        }

        private void drawEllipse(Neuron neuron, Graphics imageGraphics)
        {
            RealCoordinate coord = new RealCoordinate(neuron.synapses.Select(synapse => synapse.weight).ToArray());
            Brush brush = new SolidBrush(Color.Cyan);
            imageGraphics.FillEllipse(brush, (float)coord.x, (float)coord.y, 4F, 4F);
        }
    }
}