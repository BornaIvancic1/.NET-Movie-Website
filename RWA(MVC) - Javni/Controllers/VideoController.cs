using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWA_MVC_JAVNI.Data;
using RWA_MVC_JAVNI.Models;
using X.PagedList;
namespace RWA_MVC_JAVNI.Controllers
{
    public class VideoController : Controller
    {
        private readonly AppDbContext _context;
        public VideoController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 9;
            int pageNumber = page ?? 1;
            var videos = await _context.Video.ToPagedListAsync(pageNumber, pageSize);

            var videoViewModels = new List<VideoViewModel>();

            foreach (var video in videos)
            {
                var image = await _context.Image.FindAsync(video.ImageId);
                string imageData = image?.Content ?? string.Empty;

                var videoTags = await _context.VideoTag
                           .Where(vt => vt.VideoId == video.Id)
                           .Include(vt => vt.Tag)
                           .ToListAsync();

                var tags = videoTags.Select(vt => vt.Tag.Name).ToList();


                videoViewModels.Add(new VideoViewModel
                {
                    Video = video,
                    ImageData = imageData,
                    Tags = tags

                });
            }
            var Nad = new StaticPagedList<VideoViewModel>(videoViewModels, pageNumber, pageSize, videos.TotalItemCount);
            return View(Nad);
        }

        public async Task<IActionResult> GetData(int? page, string filter)
        {
            int pageSize = 9;
            int pageNumber = page ?? 1;

            var query = _context.Video.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(v => v.Name.Contains(filter));
            }

            var videos = await query.ToPagedListAsync(pageNumber, pageSize);

            var videoViewModels = new List<VideoViewModel>();

            foreach (var video in videos)
            {
                var image = await _context.Image.FindAsync(video.ImageId);
                string imageData = image?.Content ?? string.Empty;

                var videoTags = await _context.VideoTag
                           .Where(vt => vt.VideoId == video.Id)
                           .Include(vt => vt.Tag)
                           .ToListAsync();

                var tags = videoTags.Select(vt => vt.Tag.Name).ToList();
            

                videoViewModels.Add(new VideoViewModel
                {
                    Video = video,
                    ImageData = imageData,
                    Tags=tags
                    
                });
            }
            var Nad = new StaticPagedList<VideoViewModel>(videoViewModels, pageNumber, pageSize, videos.TotalItemCount);

            return PartialView("GetData", Nad);
        }


        public async Task<IActionResult> Details(int id)
        {
            var video = await _context.Video.FindAsync(id);

            if (video == null)
            {
                return NotFound();
            }

            var image = await _context.Image.FindAsync(video.ImageId);

            if (image == null)
            {
                return NotFound();
            }

            var videoTags = await _context.VideoTag
                .Where(vt => vt.VideoId == video.Id)
                .Include(vt => vt.Tag)
                .ToListAsync();

            var tags = videoTags.Select(vt => vt.Tag.Name).ToList();

            ViewData["Video"] = video;
            ViewData["ImageData"] = image.Content;
            ViewData["Tags"] = tags.Any() ? string.Join(", ", tags) : "No tags";

            var genre = await _context.Genre.FindAsync(video.GenreId);

            ViewData["GenreName"] = genre?.Name ?? "Unknown Genre";

            return View(video);
        }



    }
}
