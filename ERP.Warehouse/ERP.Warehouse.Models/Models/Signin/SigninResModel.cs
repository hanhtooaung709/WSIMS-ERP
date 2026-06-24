namespace ERP.Warehouse.Models.Models.Signin.Signin;

public class SigninResModel
{
    public string UserId { get; set; }
    public string SessionId { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
