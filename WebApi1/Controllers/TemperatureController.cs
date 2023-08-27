using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi1.Controllers
{
    [Route("api/temperature")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly MyContext _context;
        public TemperatureController(MyContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Get(string viewmode)
        {
            switch (viewmode)
            {
                case "realtime":
                    List<TemperatureLog> logs = new List<TemperatureLog>();
                    var log = _context.TemperatureLogs.OrderBy(e => e.Time).LastOrDefault();
                    if (log == null) { return Ok(logs); }
                    logs.Add(log);
                    return Ok(logs);
                    
                case "lastday":
                    DateTime lastDate = DateTime.Now.AddDays(-1);
                    var o1 = _context.TemperatureLogs.Where(e => e.Time > lastDate).ToList();
                    return Ok(o1);
                case "all":
                default:
                    var o = _context.TemperatureLogs.ToList();
                    return Ok(o);
            }
        }
        [HttpPost("post")]
        public async Task<IActionResult> Post(TemperatureLog log)
        {
            log.Time = DateTime.Now;
            _context.TemperatureLogs.Add(log);
            _context.SaveChanges();
            return Ok(log);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string target)
        {
            if(string.IsNullOrEmpty(target)) return NotFound();
            switch (target)
            {
                case "lastItem":
                    var log = _context.TemperatureLogs.OrderBy(e => e.Time).LastOrDefault();
                    if (log != null) { 
                        _context.TemperatureLogs.Remove(log);
                        _context.SaveChanges();
                    }
                    return Ok();

                case "lastDay":
                    DateTime lastDate = DateTime.Now.AddDays(-1);
                    var o1 = _context.TemperatureLogs.Where(e => e.Time > lastDate);
                    _context.TemperatureLogs.RemoveRange(o1);
                    _context.SaveChanges();
                    return Ok();
                case "all":
                default:
                    _context.TemperatureLogs.RemoveRange(_context.TemperatureLogs.ToList());
                    _context.SaveChanges();
                    return Ok();
            }
        }
        [HttpDelete("delete-by-date")]
        public async Task<IActionResult> DeleteByDate(string start, string end)
        {
            IQueryable<TemperatureLog> removeList = _context.TemperatureLogs.AsQueryable();
            if(DateTime.TryParse(start, out DateTime startD))
            {
                removeList = removeList.Where(e => e.Time >= startD);
            }
            if (DateTime.TryParse(end, out DateTime endD))
            {
                removeList = removeList.Where(e => e.Time <= endD);
            }
            int count = removeList.Count();
            _context.TemperatureLogs.RemoveRange(removeList);
            _context.SaveChanges();
            return Ok(new Dictionary<string, int>()
            {
                ["removedItemsCount"] = count
            });
        }
    }
}
