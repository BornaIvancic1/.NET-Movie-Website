using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using RWA_Web_API_.Data;
using RWA_Web_API_.Models;

namespace RWA_Web_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly TagContext _dbContext;

        public TagController(TagContext dbContext)
        {
            _dbContext = dbContext;
        }



        // GET: api/videos
        [HttpGet]
        public ActionResult<IEnumerable<GetTag>> GetTags()
        {
            if (_dbContext.Tag == null)
            {
                return NotFound();
            }
            List<GetTag> getTags = new List<GetTag>();
            foreach (var tag in _dbContext.Tag)
            {
                var getTag = new GetTag
                {
                    Id = tag.Id,
                    Name = tag.Name,

                };

                getTags.Add(getTag);
            }
            return getTags;
        }


        // GET: api/videos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTag>> GetTag(int id)
        {
            if (_dbContext.Tag == null)
            {
                return NotFound();
            }
            var tag = await _dbContext.Tag.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            GetTag getTag = new GetTag
            {
                Id = tag.Id,
                Name=tag.Name
            };
            return getTag;

        }

        // POST: api/videos
        [HttpPost]
        public async Task<ActionResult<Tag>> PostTag(Tag tag)
        {
          
            _dbContext.Tag.Add(tag);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag);
        }

        // PUT: api/videos/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutTag(int id, Tag tag)
        {

            var existingTag = await _dbContext.Tag.FindAsync(id);
            existingTag.Name = tag.Name;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagAvaliable(id))
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
       

        private bool TagAvaliable(int id)
        {
            return (_dbContext.Tag?.Any(x => x.Id == id)).GetValueOrDefault();
        }

        // DELETE: api/videos/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTag(int id)
        {
            if (_dbContext.Tag == null)
            {
                return NotFound();
            }
            var tag = await _dbContext.Tag.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            _dbContext.Tag.Remove(tag);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
