using Microsoft.EntityFrameworkCore;


namespace ChatApp.Data
{
    public class ChatAppContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DAL\\SQLEXPRESS;Database=ChatAppDB;Trusted_Connection=True;Encrypt=False;");
        }
    }

    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Message
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Users User { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public DateTime Created { get; set; }
        public string MessageText { get; set; }
    }

}
