using System;

namespace Example.Data
{
    public class Entity
    {
        public int Id { get; set; }
        public DateTimeOffset Modified { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}