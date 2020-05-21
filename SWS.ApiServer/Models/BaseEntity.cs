using System;

namespace SWS.ApiServer.Models
{
    public abstract class BaseEntity
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; } = new DateTime();
    }
}
