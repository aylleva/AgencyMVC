using AgencyMVC.Models.Base;

namespace AgencyMVC.Models
{
    public class Category:BaseEntity
    {
        public string Name {  get; set; }
        public List<Project> Projects { get; set; }
    }
}
