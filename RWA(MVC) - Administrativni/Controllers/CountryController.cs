using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList;
using RWA_MVC____2.Data;
using X.PagedList;

namespace RWA_MVC____2.Controllers
{
    public class CountryController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CountryController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var countries = await applicationDbContext.Country.ToPagedListAsync(pageNumber, pageSize);

            return View(countries);
        }
    }
}
