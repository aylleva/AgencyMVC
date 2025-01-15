using AgencyMVC.Models;

namespace AgencyMVC.ViewModels
{
    public class HomeVM
    {
        public List<Employee> Employees {  get; set; }
        public List<Project> Projects { get; set; }
    }
}
