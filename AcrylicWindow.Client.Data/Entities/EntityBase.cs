using MongoDB.Bson.Serialization.Attributes;

namespace AcrylicWindow.Client.Data.Entities
{
    public class EntityBase<TKey> : IEntity<TKey>
    {
        [BsonId]
        public TKey Id { get; set; }
    }
}
