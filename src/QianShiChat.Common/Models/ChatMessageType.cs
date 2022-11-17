namespace QianShiChat.Common.Models
{
    public enum ChatMessageType : sbyte
    {
        /// <summary> 文字 </summary>
        Text = 1,
        /// <summary> 图片 </summary>
        Image,
        /// <summary> 视频 </summary>
        Video,
        /// <summary> 其他文件 </summary>
        OtherFile
    }

    public enum ChatMessageSendType : sbyte
    {
        /// <summary> 个人 </summary>
        Personal = 1,
        /// <summary> 群组 </summary>
        Group
    }
}
