using AgencyMVC.Models;

namespace AgencyMVC.Areas.Admin.ViewModels.Projects
{
    public class UpdateProjectVM
    {
        public string? Name { get; set; }
        public IFormFile? Photo { get; set; }

        public int? CategoryId {  get; set; }
        public List<Category>? Categories { get; set; }
        public List<ProjectImage>? Images { get; set; }
    }
}
