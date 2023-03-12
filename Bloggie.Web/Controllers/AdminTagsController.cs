using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            await bloggieDbContext.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();
            return RedirectToAction("List");
        }

        //Veri tabanındaki verileri liste halinde tuttuk.
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var tags = await bloggieDbContext.Tags.ToListAsync();
            return View(tags);
        }

        //Edit sayfasının görüntülenme(get) actionı.
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //1.metot
            //var tag = bloggieDbContext.Tags.Find(id);

            //2.metot
            var tag = await bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);

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
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest, Guid id)
        {
            var beforeTag = await bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
            var newtag = new Tag
            {
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
            if(beforeTag != null)
            { 
            beforeTag.Name = newtag.Name;
            beforeTag.DisplayName = newtag.DisplayName;
            await bloggieDbContext.SaveChangesAsync();
                return RedirectToAction("List");
            }
            return View("Edit", new { Id = editTagRequest.Id});  
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var tag = await bloggieDbContext.Tags.FindAsync(editTagRequest.Id);
            if(tag != null)
            {
                bloggieDbContext.Remove(tag);
                await bloggieDbContext.SaveChangesAsync();
                return RedirectToAction("List");
            }
            return View();
        }


    }
}
