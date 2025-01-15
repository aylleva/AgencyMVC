using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace AgencyMVC.ViewModels
{
    public class RegisterVM
    {
        [MinLength(3)]
        [MaxLength(30)]
        public string Name {  get; set; }
        [MinLength(3)]
        [MaxLength(50)]
        public string Surname {  get; set; }
        [MaxLength(256)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [MaxLength(256)]
        [MinLength(4)]
        public string UserName {  get; set; }
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))] 
        public string ConfirmPassword {  get; set; }

    }
}
