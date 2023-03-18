using Bloggie.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Bloggie.Web.Repositories;
using Bloggie.Web.Models.ViewModels;

namespace Bloggie.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagInterface tagInterface;

        public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository, ITagInterface tagInterface)
        {
            _logger = logger;
            this.blogPostRepository = blogPostRepository;
            this.tagInterface = tagInterface;
        }

        public async Task<IActionResult> Index()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();
            var tags = await tagInterface.GetAllAsync();
            var model = new HomeViewModel
            {
                BlogPosts = blogPosts,
                Tags = tags
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}