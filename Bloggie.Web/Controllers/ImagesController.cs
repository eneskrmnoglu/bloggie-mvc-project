using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ImagesController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            //Şimdi repository çağıracağız.

        }
    }
}
