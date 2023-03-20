using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolium_Back.Models.ViewModels;
using Portfolium_Back.Services.Interfaces;

namespace Portfolium_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.userService.Get());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(String id)
        {
            return Ok(this.userService.GetById(id));
        }

        [HttpPost, AllowAnonymous]
        public IActionResult Post(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserViewModel _user = this.userService.GetOne("Email", userViewModel.Email);
            if (_user != null)
            {
                return BadRequest($"{userViewModel.Email} already in use!");
            }
            return Ok(this.userService.Post(userViewModel));
        }

        [HttpPost("authenticate"), AllowAnonymous]
        public IActionResult Authenticate(UserAuthenticateRequestViewModel userViewModel)
        {
            return Ok(this.userService.Authenticate(userViewModel));
        }
    }
}