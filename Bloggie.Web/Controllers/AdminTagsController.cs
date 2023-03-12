using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BloggieDbContext bloggieDbContext;

        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            bloggieDbContext.Add(tag);
            bloggieDbContext.SaveChanges();
            return RedirectToAction("List");
        }

        //Veri tabanındaki verileri liste halinde tuttuk.
        [HttpGet]
        public IActionResult List()
        {
            var tags = bloggieDbContext.Tags.ToList();
            return View(tags);
        }

        //Edit sayfasının görüntülenme(get) actionı.
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            //1.metot
            //var tag = bloggieDbContext.Tags.Find(id);

            //2.metot
            var tag = bloggieDbContext.Tags.FirstOrDefault(x => x.Id == id);

            if(tag != null)
            {
                var editTagReq = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagReq);
            }
            return View(null);
        }

        [HttpPost]
        public IActionResult Edit(EditTagRequest editTagRequest, Guid id)
        {
            var beforeTag = bloggieDbContext.Tags.FirstOrDefault(x => x.Id == id);
            var newtag = new Tag
            {
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
            if(beforeTag != null)
            { 
            beforeTag.Name = newtag.Name;
            beforeTag.DisplayName = newtag.DisplayName;
            bloggieDbContext.SaveChanges();
                return RedirectToAction("List");
            }
            return View("Edit", new { Id = editTagRequest.Id});

            
        }


    }
}
