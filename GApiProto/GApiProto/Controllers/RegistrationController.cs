using System;
using System.Threading.Tasks;
using GApiProto.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GApiProto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly userContext _userContext;

        public RegistrationController(ILogger<RegistrationController> logger, userContext userContext)
        {
            _logger = logger;
            _userContext = userContext;
        }

        [HttpPost]
        [AllowAnonymous] //認証無しで使えるようにする
        public async Task<ActionResult<UserProfile>> Create([FromBody] RegistrationInputModel model)
        {
            var userId = Guid.NewGuid().ToString();

            var user = new UserProfile()
            {
                UserId = userId,
                UserName = model.UserName
            };

            _userContext.Add(user);
            await _userContext.SaveChangesAsync();

            //return new CustomJsonResult(HttpStatusCode.OK, user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfile>> GetById(int id) =>
            await _userContext.UserProfile.FindAsync(id);
    }

    public class RegistrationInputModel
    {
        public string UserName { get; set; }
    }
}