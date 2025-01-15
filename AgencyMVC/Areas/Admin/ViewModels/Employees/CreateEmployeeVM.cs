using AgencyMVC.Models;

namespace AgencyMVC.Areas.Admin.ViewModels.Employees
{
    public class CreateEmployeeVM
    {
        public string Name {  get; set; }
        public IFormFile Photo {  get; set; }
        public int PositionId {  get; set; }
        public List<Position>? Positions { get; set; }

    }
}
