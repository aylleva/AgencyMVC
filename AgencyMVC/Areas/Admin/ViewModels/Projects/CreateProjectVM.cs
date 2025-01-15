using AgencyMVC.Models;

namespace AgencyMVC.Areas.Admin.ViewModels.Projects
{
    public class CreateProjectVM
    {
        public string Name { get; set; } 

        public IFormFile Photo { get; set; }
        public int CategoryId {  get; set; }
        public List<Category>? Categories { get; set; }

    }
}
