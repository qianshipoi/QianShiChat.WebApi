namespace QianShiChat.Application.Services;

public class RoomService : IRoomService, ITransient
{
    private readonly IRoomRepository _roomRepository;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RoomService(
        IRoomRepository roomRepository,
        IChatMessageRepository chatMessageRepository,
        IApplicationDbContext context,
        IMapper mapper)
    {
        _roomRepository = roomRepository;
        _chatMessageRepository = chatMessageRepository;
        _context = context;
        _mapper = mapper;
    }

    private object? FormatMessageContent(string content, ChatMessageType type)
    {
        if (type == ChatMessageType.Text)
        {
            return content;
        }

        var classType = type switch
        {
            ChatMessageType.Image => typeof(Attachment),
            ChatMessageType.Video => typeof(Attachment),
            ChatMessageType.OtherFile => typeof(Attachment),
            ChatMessageType.Audio => typeof(Attachment),
            _ => throw new NotSupportedException()
        };

        return JsonSerializer.Deserialize(content, classType);
    }

    public async Task<List<RoomDto>> GetUserRoomAsync(int userId, CancellationToken cancellationToken = default)
    {
        var rooms = await _roomRepository.GetByUser(userId).ToListAsync(cancellationToken);
        var roomDtos = new List<RoomDto>();

        foreach (var room in rooms)
        {
            var unreadCount = await _chatMessageRepository.GetUnreadCountAsync(room.Id, room.MessagePosition, cancellationToken);
            if (unreadCount > 0)
            {
                var messageDto = new RoomDto
                {
                    Id = room.Id,
                    FromId = room.FromId,
                    ToId = room.ToId,
                    Type = room.Type,
                    UnreadCount = unreadCount
                };

                // get last message content and time;
                var lastMessage = await _chatMessageRepository.GetLastMessageAsync(room.Id, cancellationToken);
                if (lastMessage is not null)
                {
                    messageDto.LastMessageTime = lastMessage.UpdateTime;
                    messageDto.LastMessageContent = FormatMessageContent(lastMessage.Content, lastMessage.MessageType);
                }

                roomDtos.Add(messageDto);
            }
        }

        return roomDtos;
    }

    public async Task<List<string>> GetRoomIdsAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _roomRepository.GetByUser(userId).AsNoTracking().Select(x => x.Id).ToListAsync(cancellationToken);
    }

    public async IAsyncEnumerable<RoomDto> GetRoomsAsync(int userId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var rooms = await _roomRepository.GetByUser(userId).ToListAsync(cancellationToken);

        foreach (var room in rooms)
        {
            var unreadCount = await _chatMessageRepository.GetUnreadCountAsync(room.Id, room.MessagePosition, cancellationToken);
            if (unreadCount > 0)
            {
                var messageDto = new RoomDto
                {
                    Id = room.Id,
                    FromId = room.FromId,
                    ToId = room.ToId,
                    Type = room.Type,
                    UnreadCount = unreadCount
                };

                // get last message content and time;
                var lastMessage = await _chatMessageRepository.GetLastMessageAsync(room.Id, cancellationToken);
                if (lastMessage is not null)
                {
                    messageDto.LastMessageTime = lastMessage.UpdateTime;
                    messageDto.LastMessageContent = FormatMessageContent(lastMessage.Content, lastMessage.MessageType);
                }
                yield return messageDto;
            }
        }
    }

    public async Task<RoomDto?> GetRoomAsync(int userId, string roomId, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.FindAsync(userId, roomId, cancellationToken);
        if (room is null)
        {
            return null;
        }

        var fromUser = await _context.UserInfos
            .AsNoTracking()
            .Where(x => x.Id == room.FromId)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        var message = await _chatMessageRepository.GetLastMessageAsync(roomId, cancellationToken);
        var roomDto = new RoomDto
        {
            Id = room.Id,
            FromId = room.FromId,
            FromUser = fromUser,
            ToId = room.ToId,
            Type = room.Type,
            UnreadCount = 0,
        };

        if (message is not null)
        {
            roomDto.LastMessageTime = message.UpdateTime;
            roomDto.LastMessageContent = FormatMessageContent(message.Content, message.MessageType);
        }

        return roomDto;
    }

    public async Task UpdateRoomPositionAsync(int userId, string roomId, long position, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.FindAsync(userId, roomId, cancellationToken);

        if (room is null) return;

        room.MessagePosition = position;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddOrUpdatePositionAsync(
        int fromId,
        int toId,
        ChatMessageSendType type,
        long position = 0,
        CancellationToken cancellationToken = default)
    {
        var roomId = string.Empty;
        if (type == ChatMessageSendType.Personal)
        {
            roomId = AppConsts.GetPrivateChatRoomId(fromId, toId);
        }
        else if (type == ChatMessageSendType.Group)
        {
            roomId = AppConsts.GetGroupChatRoomId(fromId, toId);
        }

        var room = await _roomRepository.FindAsync(fromId, roomId, cancellationToken);
        if (room is null)
        {
            room = new Room
            {
                Id = roomId,
                FromId = fromId,
                ToId = toId,
                MessagePosition = position,
            };
            await _roomRepository.AddAsync(room, cancellationToken);
        }
        else
        {
            room.MessagePosition = position;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
