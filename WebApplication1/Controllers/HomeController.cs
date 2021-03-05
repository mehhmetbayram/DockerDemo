using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileProvider _fileProvider;

        public HomeController(ILogger<HomeController> logger, IFileProvider fileProvider)
        {
            _logger = logger;
            _fileProvider = fileProvider;
        }

        public IActionResult Index()
        {

            var images = _fileProvider.GetDirectoryContents("wwwroot/images")
                .ToList()
                .Select(x=>x.Name);

            return View(images);
        }

        public IActionResult ImageSave()
        {
            return View();
        }

        public IActionResult DeleteImage(string image)
        {
            var file = _fileProvider.GetDirectoryContents("wwwroot/images")
                .FirstOrDefault(x => x.Name == image);
            System.IO.File.Delete(file.PhysicalPath);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ImageSave(IFormFile file)
        {
            if (file!=null && file.Length>0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images",fileName);

                using var stream=new FileStream(path,FileMode.Create);
                await file.CopyToAsync(stream);
            }

            return RedirectToAction("ImageSave");
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
