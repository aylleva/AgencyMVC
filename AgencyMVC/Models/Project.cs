using AgencyMVC.Models.Base;

namespace AgencyMVC.Models
{
    public class Project:BaseEntity
    {
        public string Name { get; set; }

        //relational
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<ProjectImage> ProjectImages { get; set; }
    }
}
