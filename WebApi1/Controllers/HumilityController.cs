using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi1
{
    [Route("api/humidity")]
    [ApiController]
    public class HumilityController : ControllerBase
    {
        private readonly MyContext _context;
        public HumilityController(MyContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Get(string viewmode)
        {
            switch (viewmode)
            {
                case "realtime":
                    List<HumidityLog> logs = new List<HumidityLog>();
                    var log = _context.HumidityLogs.OrderBy(e => e.Time).LastOrDefault();
                    if (log == null) { return Ok(logs); }
                    logs.Add(log);
                    return Ok(logs);

                case "lastday":
                    DateTime lastDate = DateTime.Now.AddDays(-1);
                    var o1 = _context.HumidityLogs.Where(e => e.Time > lastDate).ToList();
                    return Ok(o1);
                case "all":
                default:
                    var o = _context.HumidityLogs.ToList();
                    return Ok(o);
            }
        }
        [HttpPost("post")]
        public async Task<IActionResult> Post(HumidityLog log)
        {
            log.Time = DateTime.Now;
            _context.HumidityLogs.Add(log);
            _context.SaveChanges();
            return Ok(log);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string target)
        {
            if (string.IsNullOrEmpty(target)) return NotFound();
            switch (target)
            {
                case "lastItem":
                    var log = _context.HumidityLogs.OrderBy(e => e.Time).LastOrDefault();
                    if (log != null)
                    {
                        _context.HumidityLogs.Remove(log);
                        _context.SaveChanges();
                    }
                    return Ok();

                case "lastDay":
                    DateTime lastDate = DateTime.Now.AddDays(-1);
                    var o1 = _context.HumidityLogs.Where(e => e.Time > lastDate);
                    _context.HumidityLogs.RemoveRange(o1);
                    _context.SaveChanges();
                    return Ok();
                case "all":
                default:
                    _context.HumidityLogs.RemoveRange(_context.HumidityLogs.ToList());
                    _context.SaveChanges();
                    return Ok();
            }
        }
        [HttpDelete("delete-by-date")]
        public async Task<IActionResult> DeleteByDate(string start, string end)
        {
            IQueryable<HumidityLog> removeList = _context.HumidityLogs.AsQueryable();
            if (DateTime.TryParse(start, out DateTime startD))
            {
                removeList = removeList.Where(e => e.Time >= startD);
            }
            if (DateTime.TryParse(end, out DateTime endD))
            {
                removeList = removeList.Where(e => e.Time <= endD);
            }
            int count = removeList.Count();
            _context.HumidityLogs.RemoveRange(removeList);
            _context.SaveChanges();
            return Ok(new Dictionary<string, int>()
            {
                ["removedItemsCount"] = count
            });
        }
    }
}
