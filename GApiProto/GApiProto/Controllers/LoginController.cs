using System.Net;
using System.Threading.Tasks;
using GApiProto.Dto.Request;
using GApiProto.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GApiProto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly MemoryDatabase _memoryDatabase;

        public LoginController(UserService userService, MemoryDatabase memoryDatabase)
        {
            _userService = userService;
            _memoryDatabase = memoryDatabase;
        }

        // POST: api/Login
        [HttpPost]
        [AllowAnonymous] //ログイン機能自体は認証無しで使えるようにする
        public async Task<IActionResult> Login([FromBody] LoginPostDto loginPostDto)
        {
            var result = _userService.Login(loginPostDto).Result;
            if (result.UserProfile == null)
            {
                return new CustomJsonResult(HttpStatusCode.BadRequest, "User name or password is incorrect.");
            }

            return new CustomJsonResult(HttpStatusCode.OK, result);
        }
    }
}
