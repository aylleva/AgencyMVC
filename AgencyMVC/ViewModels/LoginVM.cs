using System.ComponentModel.DataAnnotations;

namespace AgencyMVC.ViewModels
{
    public class LoginVM
    {
        [MaxLength(256)]
        public string UserorEmail {  get; set; }
        [DataType(DataType.Password)]   
        public string Password { get; set; }
    }
}
