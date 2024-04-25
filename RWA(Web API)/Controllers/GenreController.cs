using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using RWA_Web_API_.Data;
using RWA_Web_API_.Models;

namespace RWA_Web_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly VideoContext _dbContext;

        public GenreController(VideoContext dbContext)
        {
            _dbContext = dbContext;
        }



        // GET: api/videos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
            if (_dbContext.Genre == null)
            {
                return NotFound();
            }
            return await _dbContext.Genre.ToListAsync();
        }


        // GET: api/videos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            if (_dbContext.Genre == null)
            {
                return NotFound();
            }
            var genre = await _dbContext.Genre.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            return genre;

        }

        // POST: api/videos
        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre(Genre genre)
        {
            _dbContext.Genre.Add(genre);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre);
        }

        // PUT: api/videos/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutGenre(int id, Genre genre)
        {
            var existingGenre = await _dbContext.Genre.FindAsync(id);
            existingGenre.Description = genre.Description;
            existingGenre.Name = genre.Name;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreAvaliable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return Ok();
        }

        private bool GenreAvaliable(int id)
        {
            return (_dbContext.Genre?.Any(x => x.Id == id)).GetValueOrDefault();
        }

        // DELETE: api/videos/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGenre(int id)
        {
            if (_dbContext.Genre == null)
            {
                return NotFound();
            }
            var genre = await _dbContext.Genre.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            _dbContext.Genre.Remove(genre);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
