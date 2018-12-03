using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbUsage
{
    internal class Book
    {
        public Book()
        {
            Geners = new List<string>();
        }

        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("author")]
        [BsonIgnoreIfNull]
        public string Author { get; set; }

        [BsonElement("count")]
        public int? Count { get; set; }

        [BsonElement("gener")]
        public ICollection<string> Geners { get; set; }

        [BsonElement("year")]
        public int? Year { get; set; }
    }
}
