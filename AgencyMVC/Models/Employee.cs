using AgencyMVC.Models.Base;


namespace AgencyMVC.Models
{
    public class Employee:BaseEntity
    {
        public string Name {  get; set; }

        //relational
        public int PositionId {  get; set; }
        public Position Position { get; set; }

        public List<EmployeeImage> EmployeeImages { get; set; }
    }
}
