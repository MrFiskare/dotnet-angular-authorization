using Microsoft.AspNetCore.Mvc;
using AuthorizationAPI.BindingModel;
using AuthorizationAPI.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public UserController(ILogger<UserController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpPost("RegisterUser")]
    public async Task<object> RegisterUser([FromBody] AddUpdateRegisterUserBindingModel model) 
    {
        try
        {
            var user = new AppUser()
            {
                FullName = model.FullName,
                Email = model.Email,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return await Task.FromResult("User has been registered");
            }
            return await Task.FromResult(string.Join(",", result.Errors.Select(x => x.Description.ToArray())));
        }
        catch (Exception ex)
        {

            return await Task.FromResult(ex.Message);
        }
        
    }
}
