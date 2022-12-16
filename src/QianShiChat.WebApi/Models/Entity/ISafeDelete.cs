namespace QianShiChat.WebApi.Models.Entity
{
    public interface ISafeDelete
    {
        public bool IsDeleted { get; set; }
        public long DeleteTime { get; set; }
    }
}
