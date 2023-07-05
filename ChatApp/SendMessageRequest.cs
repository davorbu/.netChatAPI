namespace ChatApp
{
    public class SendMessageRequest
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public string MessageText { get; set; }
    }
}
