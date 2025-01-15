using AgencyMVC.Models.Base;

namespace AgencyMVC.Models
{
    public class ProjectImage:BaseEntity
    {
        public string Image { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
