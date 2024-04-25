using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RWA_MVC____2.Data;
using RWA_MVC____2.Models;
using static RWA_MVC____2.Helper;

namespace RWA_MVC____2.Controllers
{
    public class GenreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GenreController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Genre
        public async Task<IActionResult> Index()
        {
              return _context.Genre != null ? 
                          View(await _context.Genre.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Genre'  is null.");
        }



        // GET: Genre/AddOrEdit
        // GET: Genre/AddOrEdit/5
        [NoDirectAccess]
        public async Task< IActionResult> AddOrEdit(int id=0)
        {
            if (id==0)
            {
                return View(new Genre());

            }
            else
            {
                var genre = await _context.Genre.FindAsync(id);
                if (genre == null)
                {
                    return NotFound();
                }
                return View(genre);
            }
        }


        // POST: Genre/AddOrEdit
 
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("Id,Name,Description")] Genre genre)
        {
          
            if (ModelState.IsValid)
            {
                if (id==0)
                {
                    _context.Add(genre);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    try
                    {
                        _context.Update(genre);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!GenreExists(genre.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                return Json(new { IsValid = true,html=Helper.RenderRazorViewToString(this,"_ViewAll",_context.Genre.ToList()) });
            }
            return Json(new { IsValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", genre )});
        }


        // POST: Genre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Genre == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Genre'  is null.");
            }
            var genre = await _context.Genre.FindAsync(id);
            if (genre != null)
            {
                _context.Genre.Remove(genre);
                await _context.SaveChangesAsync();
            }
            
       

            return Json(new {  html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Genre.ToList()) });
        }

        private bool GenreExists(int id)
        {
          return (_context.Genre?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
