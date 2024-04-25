using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NuGet.Protocol;
using RWA_Web_API_.Data;
using RWA_Web_API_.Models;

namespace RWA_Web_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly VideoContext _dbContext;

        public VideoController(VideoContext dbContext)
        {
            _dbContext = dbContext;
        }



        [HttpGet("{sortByItem}/{sortOrderItem}/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<IEnumerable<GetVideoModel>>> GetVideos(
    string sortByItem = "Name", string sortOrderItem = "asc", int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1)
            {
                return BadRequest("Page number must be greater than or equal to 1.");
            }

            if (pageSize < 1)
            {
                return BadRequest("Page size must be greater than or equal to 1.");
            }

            IList<string> sortFields = new List<string> { "Id", "CreatedAt", "Name", "Description", "GenreId", "TotalSeconds", "StreamingUrl", "ImageId" };
            IList<string> sortOrders = new List<string> { "asc", "desc" };

            if (!sortFields.Contains(sortByItem, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest("Invalid sort by field.");
            }

            if (!sortOrders.Contains(sortOrderItem.ToLower()))
            {
                return BadRequest("Invalid sort order. Use 'asc' or 'desc'.");
            }

            var videosQuery = _dbContext.Video.AsQueryable();

         
            switch (sortByItem.ToLower())
            {
                case "id":
                    videosQuery = sortOrderItem.ToLower() == "asc" ? videosQuery.OrderBy(v => v.Id) : videosQuery.OrderByDescending(v => v.Id);
                    break;
                case "createdat":
                    videosQuery = sortOrderItem.ToLower() == "asc" ? videosQuery.OrderBy(v => v.CreatedAt) : videosQuery.OrderByDescending(v => v.CreatedAt);
                    break;
                case "name":
                    videosQuery = sortOrderItem.ToLower() == "asc" ? videosQuery.OrderBy(v => v.Name) : videosQuery.OrderByDescending(v => v.Name);
                    break;
          
                default:
                    return BadRequest($"Invalid sort by field: {sortByItem}.");
            }

            var totalCount = await videosQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var videos = await videosQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var getVideoModels = new List<GetVideoModel>();

            foreach (var video in videos)
            {
                var genre = await _dbContext.Genre.FindAsync(video.GenreId);
                var image = await _dbContext.Image.FindAsync(video.imageId);

                var getVideoModel = new GetVideoModel
                {
                    Id = video.Id,
                    CreatedAt = video.CreatedAt,
                    Name = video.Name,
                    GenreId = video.GenreId,
                    imageId = video.imageId,
                    Description = video.Description,
                    TotalSeconds = video.TotalSeconds,
                    StreamingUrl = video.StreamingUrl,
                    Genre = genre,
                    Image = new Image { Content = ContainImageContent(image.Content) }
                };

                getVideoModels.Add(getVideoModel);
            }

            return Ok(new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                Videos = getVideoModels
            });
        }

        private string ContainImageContent(string content)
        {
            int maxLength = 30;
            if (content.Length > maxLength)
            {
                return content.Substring(0, maxLength) + "..."; 
            }
            return content;
        }

        // GET: api/videos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetVideoModel>> GetVideo(int id)
        {
            // Changing model to GetVideoModel so id is not a field

            if (_dbContext.Video == null)
            {
                return NotFound();
            }
            var video = await _dbContext.Video.FindAsync(id);
            if (video==null)
            {
                return NotFound();
            }
            var genre = await _dbContext.Genre.FindAsync(video.GenreId);
            video.Genre = genre;

            var image = await _dbContext.Image.FindAsync(video.imageId);
            video.Image = image;
            var getVideoModel = new GetVideoModel
            {
                Id = video.Id,
                CreatedAt = video.CreatedAt,
                Name = video.Name,
                GenreId = video.GenreId,
                imageId = video.imageId,
                Description = video.Description,
                TotalSeconds = video.TotalSeconds,
                StreamingUrl = video.StreamingUrl,
                Genre = genre,
                Image = new Image { Content = ContainImageContent(image.Content) }
            };
            return getVideoModel;
          
        } // GET: api/videos/{name}
        [HttpGet("search/{name}")]
        public async Task<ActionResult<List<GetVideoModel>>> GetVideosByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required.");
            }

            var videos = await _dbContext.Video
                .Where(v => v.Name.Contains(name))
                .ToListAsync();

            if (videos == null || videos.Count == 0)
            {
                return NotFound("No videos found with the specified name.");
            }

            var getVideoModels = new List<GetVideoModel>();

            foreach (var video in videos)
            {
                var genre = await _dbContext.Genre.FindAsync(video.GenreId);
                var image = await _dbContext.Image.FindAsync(video.imageId);

                var getVideoModel = new GetVideoModel
                {
                    Id = video.Id,
                    CreatedAt = video.CreatedAt,
                    Name = video.Name,
                    GenreId = video.GenreId,
                    imageId = video.imageId,
                    Description = video.Description,
                    TotalSeconds = video.TotalSeconds,
                    StreamingUrl = video.StreamingUrl,
                    Genre = genre,
                    Image = new Image { Content = ContainImageContent(image.Content) }
                };

                getVideoModels.Add(getVideoModel);
            }

            return getVideoModels;
        }


        // POST: api/videos
        [HttpPost]
        public async Task<ActionResult<Video>> PostVideo(Video video)
        {
        
            if (!_dbContext.Genre.Any(g => g.Name == video.Genre.Name))
            {
                var newGenre = video.Genre;
              

                _dbContext.Genre.Add(newGenre);
                await _dbContext.SaveChangesAsync();

            
                video.GenreId = newGenre.Id;
            }

            if (!_dbContext.Image.Any(i => i.Content == video.Image.Content))
            {
                var newImage = video.Image;
                int maxImageId = _dbContext.Image.Max(i => i.Id);
                newImage.Id = maxImageId + 1;
                _dbContext.Image.Add(newImage);
                await _dbContext.SaveChangesAsync();

          
                video.imageId = newImage.Id;
            }
            video.CreatedAt = DateTime.Now;


            _dbContext.Video.Add(video);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVideo), new { id = video.Id }, video);
        }

        // PUT: api/videos/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutVideo(int id, PostVideoModel Video)
        {

            // Changing model to PostVideoModel so it is not able to modify image or genre
            var existingVideo = await _dbContext.Video.FindAsync(id);
            
            if (existingVideo == null)
            {
                return NotFound("Video not found");
            }
        

            existingVideo.Name = Video.Name;
            existingVideo.Description = Video.Description;
            existingVideo.GenreId = Video.GenreId;
            existingVideo.TotalSeconds = Video.TotalSeconds;
            existingVideo.StreamingUrl = Video.StreamingUrl;
            existingVideo.imageId = Video.imageId;
          

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(id))
                {
                    return NotFound("Video not found");
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        private bool VideoExists(int id)
        {
            return _dbContext.Video.Any(e => e.Id == id);
        }


        // DELETE: api/videos/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVideo(int id)
        {
            if (_dbContext.Video==null)
            {
                return NotFound();
            }
            var video = await _dbContext.Video.FindAsync(id);
            if (video==null)
            {
                return NotFound();
            }
            _dbContext.Video.Remove(video);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
