using System;
using System.Collections.Generic;
using System.Drawing;

namespace KohonenMVC.Kohonen
{
    class DataLoader
    {
        private string imagePath;

        public DataLoader(string imagePath)
        {
            this.imagePath = imagePath;
        }

        public Data[] loadData(out int imageWidth, out int imageHeight)
        {
            Bitmap image = new Bitmap(imagePath);
            List<Data> trainingData = new List<Data>();
            imageWidth = image.Width;
            imageHeight = image.Height;

            for (int x = 0; x < image.Width; x++)
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    if (pixelColor.Name != "ffffffff")
                        trainingData.Add(new Data(new int[] { x, y }));
                }

            return shuffleData(trainingData.ToArray());
        }

        private static Data[] shuffleData(Data[] data)
        {
            Data[] shuffledData = data;
            for (int i = 0; i < data.Length; i++)
            {
                Random generator = new Random();
                int random;
                bool uniqueNumberGenerated;
                do
                {
                    random = generator.Next(data.Length);
                    uniqueNumberGenerated = false;
                    if (i != random)
                    {
                        Data temp;
                        temp = shuffledData[random];
                        shuffledData[random] = shuffledData[i];
                        shuffledData[i] = temp;
                        uniqueNumberGenerated = true;
                    }
                } while (uniqueNumberGenerated);
            }
            return shuffledData;
        }
    }
}
