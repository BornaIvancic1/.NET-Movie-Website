using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWA_MVC____2.Data;
using RWA_MVC____2.Models;
using X.PagedList;

public class VideoController : Controller
{
    private readonly ApplicationDbContext _applicationDbContext;

    public VideoController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<IActionResult> Index(int? page)
    {
        int pageSize = 5;
        int pageNumber = page ?? 1;
        var videos = await _applicationDbContext.Video.ToPagedListAsync(pageNumber, pageSize);
        return View(videos);
    }
    public async Task<IActionResult> GetData(int? page, string filterVideo,string filterGenre)
    {
        int pageSize = 5;
        int pageNumber = page ?? 1;

        var query = _applicationDbContext.Video.Include(v => v.Genre).AsQueryable();


        if (!string.IsNullOrEmpty(filterVideo))
        {
            query = query.Where(v => v.Name.Contains(filterVideo));
        }
        if (!string.IsNullOrEmpty(filterGenre))
        {
            query = query.Where(v => v.Genre.Name.Contains(filterGenre));
        }
        var genres = await _applicationDbContext.Genre.ToListAsync();
        ViewData["Genres"] = genres;
        var videos = await query.ToPagedListAsync(pageNumber, pageSize);
        return PartialView("GetData", videos);
    }
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Add(Video addVidoeRequest)
    {
        addVidoeRequest.CreatedAt = DateTime.Now;
        await _applicationDbContext.Video.AddAsync(addVidoeRequest);
        await _applicationDbContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]

    public async Task<IActionResult> View(int id)
    {

        var video = await _applicationDbContext.Video.FirstOrDefaultAsync(x => x.Id == id);

        if (video != null)
        {
            var viewModel = new Video()
            {
                Id = video.Id,
                Name = video.Name,
                Description = video.Description,
                GenreId = video.GenreId,
                TotalSeconds = video.TotalSeconds,
                StreamingUrl = video.StreamingUrl,
                ImageId = video.ImageId
            };

            return await Task.Run(() => View("View", viewModel));
        }



        return RedirectToAction("Index");
    }

    [HttpPost]

    public async Task<IActionResult> View(Video model)
    {

        var video = await _applicationDbContext.Video.FindAsync(model.Id);

        if (video != null)
        {
            video.Name = model.Name;
            video.Description = model.Description;
            video.GenreId = model.GenreId;
            video.TotalSeconds = model.TotalSeconds;
            video.StreamingUrl = model.StreamingUrl;
            video.ImageId = model.ImageId;



            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Video model)
    {
        var video = await _applicationDbContext.Video.FindAsync(model.Id);

        if (video != null)
        {
            _applicationDbContext.Video.Remove(video);

            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");


    }


}
