using System.ComponentModel.DataAnnotations;

namespace Maxim.Areas.Manage.ViewModels.Account
{
    public class RegisterVM
    {
        [MinLength(3)]
        public string Name { get; set; }
        [MaxLength(25)]
        public string Surname { get; set; }
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password),Compare("Password")]
        public string ComfirmPassword { get; set; }
    }
}
