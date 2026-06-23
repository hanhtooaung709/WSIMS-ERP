using Microsoft.AspNetCore.Mvc;
using ERP.Warehouse.Api.BaseController;
using ERP.Warehouse.Api.Features.SignIn;
using ERP.Warehouse.Models.Models.Signin.Signin;

[Route("api/warehouse-user")]
[ApiController]
public class SignInController : BaseController
{
    private readonly SignInService _signInService;

    public SignInController(SignInService signInService)
    {
        _signInService = signInService;
    }

    [HttpPost]
    [Route("SignIn")]
    public async Task<IActionResult> SignIn(SigninReqModel reqModel)
    {
        var result = await _signInService.SignIn(reqModel);
        return Execute(result);
    }
}
