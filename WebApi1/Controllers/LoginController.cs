using Microsoft.AspNetCore.Mvc;

namespace WebApi1.Controllers
{
   
    public class LoginStatus
    {
        public bool IsSuccess { get; set; }
        //public bool IsWrongUsername { get; set; }
        //public bool IsPassword { get; set; }
    }
    public class RegisterStatus
    {
        public bool IsSuccess { get; set; }
    }
    [Route("user")]
    public class LoginController : Controller
    {
        private readonly MyContext _context;
        public LoginController(MyContext context)
        {
            _context = context;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginInfo info)
        {
            LoginStatus loginStatus = new LoginStatus();
            loginStatus.IsSuccess = _context.LoginInfos.Any(i => i.Username == info.Username && i.Password == info.Password);
            return Ok(loginStatus);
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] LoginInfo info)
        {
            RegisterStatus registerStatus = new RegisterStatus();
            registerStatus.IsSuccess = _context.LoginInfos.Any(i => i.Username == info.Username);
            if (!registerStatus.IsSuccess)
            {
                registerStatus.IsSuccess = true;
                _context.LoginInfos.Add(info);
                _context.SaveChanges();
            }
            else
                registerStatus.IsSuccess = false;
            return Ok(registerStatus);
        }
        [HttpGet("getaccs")]
        public IActionResult GetAllAccounts()
        {
            var list = _context.LoginInfos.ToArray();
            return Ok(list);
        }
    }
}
