using System.ComponentModel.DataAnnotations;

namespace KohonenMVC.Models
{
    public class TrainingModel
    {
        [Required(ErrorMessage = "Please enter real numnber between 0 and 1.")]
        [Display(Name = "Adaptive height")]
        public double adaptiveHeight { get; set; }
        [Range(0, 1)]

        [Required(ErrorMessage = "Please enter small real number.")]
        [Display(Name = "Learning parameter")]
        public double learningParameter { get; set; }

        [Required(ErrorMessage = "Please enter an integer value.")]
        [Display(Name = "Kohonen layer width")]
        public int kohonenLayerWidth { get; set; }

        [Required(ErrorMessage = "Please enter an integer value.")]
        [Display(Name = "Kohonen layer length")]
        public int kohonenLayerLength { get; set; }

        [Required(ErrorMessage = "Please enter an integer value of 5 or more")]
        [Display(Name = "Number of iterations")]
        public int numberOfIterations { get; set; }
        [Range(5,100000)]

        [Required(ErrorMessage = "Please enter name of folder where training data is located.")]
        [Display(Name = "Training data folder")]
        public string folderName { get; set; }

        [Required(ErrorMessage ="Please enter name of training data file")]
        [Display(Name ="Training data file")]
        public string fileName { get; set; }

    }
}