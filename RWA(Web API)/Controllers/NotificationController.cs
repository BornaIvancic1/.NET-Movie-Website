using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWA_Web_API_.Data;
using RWA_Web_API_.Models;

namespace RWA_Web_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationContext _dbContext;

        public NotificationController(NotificationContext dbContext)
        {
            _dbContext = dbContext;
        }



        // GET: api/videos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
        {
            if (_dbContext.Notification == null)
            {
                return NotFound();
            }
            return await _dbContext.Notification.ToListAsync();
        }


        // GET: api/videos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(int id)
        {
            if (_dbContext.Notification == null)
            {
                return NotFound();
            }
            var notification = await _dbContext.Notification.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            return notification;

        }
        // GET: api/notification/count
        [HttpGet("count")]
        public ActionResult<int> GetPendingNotificationsCount()
        {
            var count = _dbContext.Notification.Count(notification => notification.SentAt == DateTime.MinValue);
            return count;
        }

        // POST: api/videos
        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotification(PostNotification postNotification)
        {
            Notification notification = new Notification
            {
                Id = postNotification.Id,   
                 Body=postNotification.Body,
                  SentAt = DateTime.MinValue,
                   CreatedAt = DateTime.Now,   
                   Subject=postNotification.Subject,
                    ReceiverEmail=postNotification.ReceiverEmail,
            };
          
            _dbContext.Notification.Add(notification);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetNotification), new { id = notification.Id }, notification);
        }

        // PUT: api/videos/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutNotification(int id, PostNotification notification)
        {

            var existingNotification = await _dbContext.Notification.FindAsync(id);
            existingNotification.Body = notification.Body;
            existingNotification.Subject = notification.Subject;
            existingNotification.ReceiverEmail = notification.ReceiverEmail;
         
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationAvaliable(id))
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

        private bool NotificationAvaliable(int id)
        {
            return (_dbContext.Notification?.Any(x => x.Id == id)).GetValueOrDefault();
        }

        // DELETE: api/videos/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNotification(int id)
        {
            if (_dbContext.Notification == null)
            {
                return NotFound();
            }
            var notification = await _dbContext.Notification.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            _dbContext.Notification.Remove(notification);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
        [HttpPost("sendAll")]
        public async Task<ActionResult> SendNotifications()
        {
            try
            {
                var unsentNotifications = _dbContext.Notification.Where(n => n.SentAt == DateTime.MinValue || n.SentAt > DateTime.Now).ToList();

                foreach (var notification in unsentNotifications)
                {
                    await SendEmailNotification(notification);
                    notification.SentAt = DateTime.Now;
                    _dbContext.Entry(notification).State = EntityState.Modified;
                }

                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch 
            {
                return StatusCode(500, "Error sending notifications");
            }
        }
        
        private async Task SendEmailNotification(Notification notification)
        {
            try
            {
                var smtpServer = "smtp.gmail.com";
                var smtpPort = 587;

                var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential("obavijestirwa@gmail.com", "tgxs pxwl obfm ioxp"),
                    EnableSsl = true,
                     
                };

                

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("obavijestirwa@gmail.com"),
                    Subject = notification.Subject,
                    Body = notification.Body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(notification.ReceiverEmail); 

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch
            {
          
            }
        }

    }
}
