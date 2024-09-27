using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProject.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage ="New password Is Required")]
        [DataType(DataType.Password) ]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm New password Is Required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword" , ErrorMessage =" Password Doesn't match") ]
        public string ConfirmPassword { get; set; }
    }
}
