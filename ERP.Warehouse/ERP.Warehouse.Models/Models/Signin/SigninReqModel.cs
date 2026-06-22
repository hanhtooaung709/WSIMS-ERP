using System.ComponentModel.DataAnnotations;

namespace ERP.Warehouse.Models.Models.Signin.Signin;

public class SigninReqModel
{
    [Required(ErrorMessage = "Please enter your UserName.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Passcode must be 6 digits.")]
    public string Password { get; set; }
}

public class SignoutReqModel
{
    public string UserName { get; set; } = string.Empty;

    public string SessionToken { get; set; } = string.Empty;
}
