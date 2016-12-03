using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Helpers;
using System.IO;
using System.Drawing;
using KohonenMVC.Kohonen;
using KohonenMVC.Models;

namespace KohonenMVC.Controllers
{
    public class TrainingController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(TrainingModel form)
        {
            if (form != null)
            {
                TempData["form"] = form;
                return RedirectToAction("TrainNetwork");
            }

            return View();
        }

        public ActionResult TrainNetwork()
        {
            TrainingModel form = TempData["form"] as TrainingModel;
            var dirPath = Path.Combine(Server.MapPath("~/App_Data/"), form.folderName);
            List<string> files = Directory.GetFiles(dirPath).ToList();

            string imagePath = Path.Combine(dirPath, files.Find(s => s.Contains(form.fileName)));

            KohonenTraining training = new KohonenTraining(form.adaptiveHeight, form.learningParameter,
                form.kohonenLayerWidth, form.kohonenLayerLength, imagePath, form.numberOfIterations, dirPath);

            Bitmap picture = training.trainNetwork();
            string tempPath = Path.Combine(Server.MapPath("~/Content/"), "drawImage.jpg");
            string picturePathSuffixed = Path.Combine(Server.MapPath("~/App_Data/"), "drawImage.jpg");
            string picturePath = Path.Combine(Server.MapPath("~/App_Data/"), "drawImage");
            System.IO.File.Delete(picturePathSuffixed);
            picture.Save(picturePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            picture.Save(tempPath, System.Drawing.Imaging.ImageFormat.Jpeg);

            string chartFileName = "chart.txt";
            string chartFilePath = Path.Combine(Server.MapPath("~/App_Data/"), chartFileName);
            FileStream stream = new FileStream(chartFilePath, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);

            if (training.learning.maximumWeightChanges.Length < 100)
            {
                for (int i = 0; i < training.learning.maximumWeightChanges.Length; i++)
                    writer.WriteLine((i + 1) + " " + training.learning.maximumWeightChanges[i]);
            }
            else
            {
                for (int i = training.learning.maximumWeightChanges.Length - 100; i < training.learning.maximumWeightChanges.Length; i++)
                    writer.WriteLine((i + 1) + " " + training.learning.maximumWeightChanges[i]);
            }

            writer.Close();
            stream.Close();

            return View(form);
        }

        public ActionResult WeightsChangeChart()
        {
            string chartFilePath = Path.Combine(Server.MapPath("~/App_Data/"), "chart.txt");
            FileStream stream = new FileStream(chartFilePath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            List<int> xAxisData = new List<int>();
            List<double> yAxisData = new List<double>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] lineParts = line.Trim(' ').Split(' ');
                xAxisData.Add(int.Parse(lineParts[0]));
                yAxisData.Add(double.Parse(lineParts[1]));
            }
            reader.Close();
            stream.Close();

            System.IO.File.Delete(chartFilePath);

            Chart weightsChart = new Chart(width: 600, height: 400)
            .AddTitle("Progress of global error and learning success")
            .AddSeries(
                name: "Maximum Weight Change",
                chartType: "Line",
                xField: "Iteration",
                xValue: xAxisData,
                yFields: "Maximum weight change",
                yValues: yAxisData)
            .AddLegend()
            .Write();

            weightsChart.Save("~/Content/chart", "jpeg");
            return File("~/Content/chart", "jpeg");
        }

        public ActionResult GetBitmap()
        {
            return File("~/App_Data/drawImage", "jpeg");
        }
    }
}