using System.Drawing;

namespace KohonenMVC.Kohonen
{
    public class KohonenTraining
    {
        private double adaptiveHeight;
        private double learningParameter;
        private int kohonenLayerWidth;
        private int kohonenLayerLength;
        private int numberOfIterations;
        private string filePath;
        private string folderPath;
        public Learning learning { get;  private set; }

        public KohonenTraining(double adaptiveHeight, double learningParameter, int kohonenLayerWidth, 
            int kohonenLayerLength, string filePath, int numberOfIterations, string folderPath)
        {
            this.adaptiveHeight = adaptiveHeight;
            this.learningParameter = learningParameter;
            this.kohonenLayerLength = kohonenLayerLength;
            this.kohonenLayerWidth = kohonenLayerWidth;
            this.numberOfIterations = numberOfIterations;
            this.filePath = filePath;
            this.folderPath = folderPath;
        }

        public Bitmap trainNetwork()
        {
            DataLoader loader = new DataLoader(filePath);
            int imageWidth, imageHeight;
            Data[] trainingData = loader.loadData(out imageWidth, out imageHeight);
            Initialiser init = new Initialiser(2, learningParameter, adaptiveHeight, kohonenLayerWidth, kohonenLayerLength);
            Network network = init.createKohonenNetwork();
            init.initialiseKohonenNetwork(network, trainingData);
            learning = new Learning(network, trainingData, numberOfIterations, new int[] { imageWidth, imageHeight});
            learning.folderPath = folderPath;
            learning.executeTrainingCycle();
            Result result = new Result(filePath, network);
            return result.createAndSaveResult();
        }
    }
}