using System.ComponentModel.DataAnnotations;

namespace ConstructionApplication.ViewModels
{
    public class CostMasterVm
    {
        public int Id { get; set; }
        public int JobCategoryId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }

    public class AddNewCostMasterVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "JobCategory is invalid")]
        public int? JobCategoryId { get; set; }

        [Required(ErrorMessage = "Cost is invalid")]
        public decimal? Cost { get; set; }

        [Required(ErrorMessage = "Date is invalid")]
        public DateTime? Date { get; set; }

    }
}
