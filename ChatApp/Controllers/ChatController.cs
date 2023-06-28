using ChatApp;
using ChatApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatAppContext _context;

        public ChatController(ChatAppContext context)
        {
            _context = context;
        }

        [HttpPost("send-message")]
        [Authorize]
        public IActionResult SendMessage(SendMessageRequest request)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Invalid user");

            var room = _context.Room.Find(request.RoomId);
            if (room == null)
                return BadRequest("Room doesn't exist");

            var newMessage = new Message
            {
                UserId = int.Parse(userId),
                RoomId = request.RoomId,
                Created = DateTime.Now,
                MessageText = request.MessageText
            };

            _context.Messages.Add(newMessage);
            _context.SaveChanges();

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
          
            HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            return Ok("Message added");
        }





        [HttpGet("get-messages")]
        public IActionResult GetMessages(int roomId)
        {
            var room = _context.Room.Find(roomId);
            if (room == null)
                return BadRequest("Room doesn't exist");

            var messages = _context.Messages
                .Where(m => m.RoomId == roomId)
                .Select(m => new
                {
                    messageId = m.Id,
                    userId = m.UserId,
                    message = m.MessageText,
                    created = m.Created
                })
                .ToList();

            return Ok(messages);
        }
    }
}
