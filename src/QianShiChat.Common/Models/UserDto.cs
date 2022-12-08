namespace QianShiChat.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public string Avatar { get; set; }

        public string NickName { get; set; }

        public long CreateTime { get; set; }
    }

    public class UserWithMessage : UserDto
    {
        public List<ChatMessageDto> Messages { get; set; }
    }
}