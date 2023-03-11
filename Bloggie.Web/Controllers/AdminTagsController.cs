using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")] //Data annotation ile farklı bir isim verirken aynı action'ı çalıştırması gerekiyor. O yüzden bu şekilde kullanım yapıyoruz.
        public IActionResult SubmitTag()
        {
            var name = Request.Form["name"]; // Formdan gelen verileri tuttuk.
            var displayName = Request.Form["displayName"];

            return View("Add");
        }


    }
}
