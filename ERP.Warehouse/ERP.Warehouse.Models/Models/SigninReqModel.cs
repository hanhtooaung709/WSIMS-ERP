using System.ComponentModel.DataAnnotations;

namespace ERP.Warehouse.Models.Models;

public class SigninReqModel
{
    [Required(ErrorMessage = "Please enter your UserName.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Passcode must be 6 digits.")]
    public string Password { get; set; }
}
