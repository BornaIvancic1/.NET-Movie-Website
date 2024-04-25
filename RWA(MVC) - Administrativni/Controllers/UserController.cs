using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWA_MVC____2.Data;
using RWA_MVC____2.Models;

using X.PagedList;

namespace RWA_MVC____2.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public UserController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var users = await applicationDbContext.User.ToPagedListAsync(pageNumber, pageSize);
            return View(users);
        }
        public async Task<IActionResult> GetData(int? page, string firstNameFilter, string lastNameFilter, string usernameFilter, string countryFilter)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var query = applicationDbContext.User.Include(u=>u.CountryOfResidence).AsQueryable();

            if (!string.IsNullOrEmpty(firstNameFilter))
            {
                query = query.Where(u => u.FirstName.Contains(firstNameFilter));
            }

            if (!string.IsNullOrEmpty(lastNameFilter))
            {
                query = query.Where(u => u.LastName.Contains(lastNameFilter));
            }

            if (!string.IsNullOrEmpty(usernameFilter))
            {
                query = query.Where(u => u.Username.Contains(usernameFilter));
            }  
            if (!string.IsNullOrEmpty(countryFilter))
            {
             
                query = query.Where(u => u.CountryOfResidence.Name.Contains(countryFilter));
            }

     
            var countries = await applicationDbContext.Country.ToListAsync();
            ViewData["Countries"] = countries;
            var users = await query.ToPagedListAsync(pageNumber, pageSize);
            return PartialView("GetData", users);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(User addUserRequest)
        {

            string salt = Encryption.CreateSalt(8);
            string password = addUserRequest.PwdHash;
            var user = new User()
            {
                Username=addUserRequest.Username,
                FirstName=addUserRequest.FirstName,
                LastName=addUserRequest.LastName,
                Email=addUserRequest.Email,
                Phone=addUserRequest.Phone,
                 CreatedAt= DateTime.Now,
                PwdSalt= salt,
                PwdHash=Encryption.GenerateHash(password,salt),
                 CountryOfResidenceId=addUserRequest.CountryOfResidenceId
            };

            await applicationDbContext.User.AddAsync(user);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]

        public async Task<IActionResult> View(int id)
        {

            var user = await applicationDbContext.User.FirstOrDefaultAsync(x => x.Id == id);

            if (user != null)
            {
                var viewModel = new User()
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    CountryOfResidenceId=user.CountryOfResidenceId
                };

                return await Task.Run(() => View("View", viewModel));
            }



            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> View(User model)
        {

            var user = await applicationDbContext.User.FindAsync(model.Id);

            if (user != null)
            {
                user.Username = model.Username;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Phone = model.Phone;
                user.CountryOfResidenceId = model.CountryOfResidenceId;
                    
              

                await applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(User model)
        {
            var user = await applicationDbContext.User.FindAsync(model.Id);

            if (user != null)
            {
                user.DeletedAt = DateTime.Now;

                await applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");


        }

       

    }
}
