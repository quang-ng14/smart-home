using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi1.Controllers
{
    [Route("api/remote")]
    [ApiController]
    public class RemoteController : ControllerBase
    {
        private static Dictionary<string, bool> devicesState;
        public RemoteController(MyContext context)
        {
            if(devicesState == null)
            {
                devicesState = new Dictionary<string, bool>();
                devicesState.Add("light1", false);
                devicesState.Add("light2", false);
                devicesState.Add("engine1", false);
                devicesState.Add("fan1", false);
            }
        }
        [HttpGet("currentstates")]
        public async Task<IActionResult> Get()
        {
            return Ok(devicesState);
            
        }
        [HttpPost("change")]
        public async Task<IActionResult> Post(Dictionary<string, bool> states)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            bool stateOnCloud;
            foreach(var device in states.Keys)
            {
                bool stateOnClient = states[device];
                if(devicesState.TryGetValue(device, out stateOnCloud))
                {
                    if(stateOnCloud != stateOnClient)
                    {
                        devicesState[device] = stateOnClient;
                        result[device] = "changed";
                    }
                    else result[device] = "sameAsCloud";
                }
                else result[device] = "deviceNotFound";
            }
            return Ok(result);
        }
    }
}
