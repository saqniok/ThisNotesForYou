using LiteDB;

namespace ThisNotesForYou;
    public class Note
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
