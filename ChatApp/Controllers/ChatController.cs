using ChatApp.Data;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult SendMessage(int userId, int roomId, string messageText)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
                return BadRequest("User doesn't exist");

            var room = _context.Room.Find(roomId);
            if (room == null)
                return BadRequest("Room doesn't exist");

            var newMessage = new Message
            {
                UserId = userId,
                RoomId = roomId,
                Created = DateTime.Now,
                MessageText = messageText
            };

            _context.Messages.Add(newMessage);
            _context.SaveChanges();

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

