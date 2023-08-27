using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace WebApi1
{
    [Route("")]
    [ApiController]
    public class WebsiteController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Download()
        {
            Stream stream = System.IO.File.Open("Contents/index.html", FileMode.Open);

            if (stream == null)
                return NotFound(); // returns a NotFoundResult with Status404NotFound response.

            return File(stream, "text/html"); // returns a FileStreamResult
        }
        string GetFileType(string filename)
        {
            //string fullExtension = filename.Substring(filename.IndexOf('.') + 1);
            string mainExtension = filename.Substring((filename.LastIndexOf('.') + 1));
            switch (mainExtension)
            {
                case "html":
                    return "text/html";
                case "css":
                    return "text/css";
                case "js":
                    return "application/javascript";
                case "json":
                    return "application/json";
                case "txt":
                    return "text/plain";
                default:
                    return "application/x-binary";
            }
        }
        [HttpGet("{filename}")]
        public async Task<IActionResult> DownloadContents(string filename)
        {
            if (!System.IO.File.Exists("Contents/" + filename))
                return NotFound(); // returns a NotFoundResult with Status404NotFound response.
            Stream stream = System.IO.File.Open("Contents/" + filename, FileMode.Open);

            return File(stream, GetFileType(filename)); // returns a FileStreamResult
        }
    }
}
