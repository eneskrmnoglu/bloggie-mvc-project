using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Bloggie.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagInterface tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagInterface tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await tagRepository.GetAllAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Author = addBlogPostRequest.Author,
                Content = addBlogPostRequest.Content,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Visible = addBlogPostRequest.Visible,
                ShortDescription = addBlogPostRequest.ShortDescription,
                UrlHandle = addBlogPostRequest.UrlHandle,
            };

            //Şimdi de tagleri mapleyelim.
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            //Blogpostumuzun taglerine bu tagleri eşitleyelim.
            blogPost.Tags = selectedTags;

            await blogPostRepository.AddAsync(blogPost);

            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogPost = await blogPostRepository.GetAllAsync();
            return View(blogPost);
        }

        [HttpGet]

        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await blogPostRepository.GetAsync(id);
            var tagsFromDomainModel = await tagRepository.GetAllAsync();

            if (blogPost != null)
            {
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    Heading = blogPost.Heading,
                    ShortDescription = blogPost.ShortDescription,
                    Tags = tagsFromDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }
            return View(null);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Visible = editBlogPostRequest.Visible,
                PageTitle = editBlogPostRequest.PageTitle,
                Author = editBlogPostRequest.Author,
                Content = editBlogPostRequest.Content,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Heading = editBlogPostRequest.Heading,
                ShortDescription = editBlogPostRequest.ShortDescription,
            };

            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }
            blogPostDomainModel.Tags = selectedTags;

            // Submit işlemi
            var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);
            if (updatedBlog != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            var deletedPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
            if (deletedPost != null)
            {

                //success notification
                return RedirectToAction("List");
            }

            //error notification
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
        }

    }

}
