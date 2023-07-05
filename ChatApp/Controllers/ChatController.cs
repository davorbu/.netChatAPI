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
        public IActionResult SendMessage(SendMessageRequest parms)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var user = _context.Users.Find(int.Parse(userId));
            

            var newMessage = new Message
            {
                UserId = parms.UserId, 
                RoomId = parms.RoomId,
                Created = DateTime.Now,
                MessageText = parms.MessageText
            };

            _context.Messages.Add(newMessage);
            _context.SaveChanges();

            return Ok("Message added");
        }



        [HttpGet("get-messages")]
        [Authorize]
        public IActionResult GetMessages(int roomId)
        {
            var room = _context.Rooms.Find(roomId);
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
