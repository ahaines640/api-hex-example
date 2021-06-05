using System;

namespace Example.Api.Tests.Acceptance.Data
{
    public class Entity
    {
        public int Id { get; set; }
        public DateTimeOffset Modified { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; } = "Acceptance Test";
        public bool IsDeleted { get; set; }
    }
}