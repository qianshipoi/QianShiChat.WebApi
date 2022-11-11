namespace QianShiChat.Models
{
    /// <summary>
    /// 申请状态
    /// </summary>
    public enum ApplyStatus : sbyte
    {
        /// <summary>
        /// 已申请
        /// </summary>
        Applied = 1,
        /// <summary>
        /// 通过
        /// </summary>
        Passed,
        /// <summary>
        /// 驳回
        /// </summary>
        Rejected,
        /// <summary>
        /// 忽略
        /// </summary>
        Ignored
    }
}
