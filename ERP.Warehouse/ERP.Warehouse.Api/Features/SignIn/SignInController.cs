using Microsoft.AspNetCore.Mvc;
using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.SignIn;
using ERP.Warehouse.Models.Models.Signin.Signin;
using ERP.Warehouse.Models.Models.Signin;
using Microsoft.AspNetCore.Authorization;
using WSIMS_ERP.Shared.Services;

[Route("api/warehouse-user")]
[ApiController]
public class SignInController : BaseController
{
    private readonly SignInService _signInService;

    public SignInController(SignInService signInService, ResponseService responseService) : base(responseService)
    {
        _signInService = signInService;
    }

    [HttpPost]
    [Route("SignIn")]
    public async Task<IActionResult> SignIn(SigninReqModel reqModel)
    {
        var result = await _signInService.SignIn(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Authorize]
    [Route("Logout")]
    public async Task<IActionResult> Logout(LogoutReqModel reqModel)
    {
        var result = await _signInService.Logout(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Authorize]
    [Route("GetUserData")]
    public async Task<IActionResult> GetUserData()
    {
        var result = await _signInService.GetUserData();
        return await Execute(result);
    }
}
