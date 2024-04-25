using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWA_MVC____2.Data;
using RWA_MVC____2.Models;

namespace RWA_MVC____2.Controllers
{
    public class TagController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public TagController(ApplicationDbContext  applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tags= await applicationDbContext.Tag.ToListAsync();
            return View(tags);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(AddTagViewModel addTagRequest)
        {
            var tag=new Tag() { 
            Name=addTagRequest.Name
            };

         await   applicationDbContext.Tag.AddAsync(tag);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]

        public async Task<IActionResult> View(int id) {

            var tag=await applicationDbContext.Tag.FirstOrDefaultAsync(x=>x.Id==id);

            if (tag!=null)
            {
                var viewModel = new UpdateTagViewModel()
                {
                    Id = tag.Id,
                    Name = tag.Name
                };

                return await Task.Run(()=> View("View",viewModel));
            }



            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> View(UpdateTagViewModel model) {

            var tag = await applicationDbContext.Tag.FindAsync(model.Id);

            if (tag!=null)
            {
                tag.Name = model.Name;

                await applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateTagViewModel model)
        {
            var tag = await applicationDbContext.Tag.FindAsync(model.Id);
            var videoTag = await applicationDbContext.VideoTag
     .FirstOrDefaultAsync(vt => vt.TagId == model.Id );
            if (videoTag != null)
            {
                applicationDbContext.VideoTag.Remove(videoTag);

                await applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            if (tag != null)
            {
                applicationDbContext.Tag.Remove(tag);

                await applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }   
            return RedirectToAction("Index");


        }

    }
}
