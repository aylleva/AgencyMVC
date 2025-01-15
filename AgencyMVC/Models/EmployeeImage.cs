using AgencyMVC.Models.Base;

namespace AgencyMVC.Models
{
    public class EmployeeImage:BaseEntity
    {
        public string Image {  get; set; }
        public int EmployeeId {  get; set; }
        public Employee Employee { get; set; }
    }
}
