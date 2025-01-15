using AgencyMVC.Models;
using Microsoft.AspNetCore.Http.Metadata;

namespace AgencyMVC.Areas.Admin.ViewModels.Employees
{
    public class UpdateEmployeeVM
    {
        public string? Name {  get; set; }
        public IFormFile? Photo { get; set; }
        public List<EmployeeImage>? Images {  get; set; }
        public int? PositionId {  get; set; }
        public List<Position>? Positions { get; set; }
    }
}
