using System;

namespace SWS.Models.DbModels
{
    public class BaseEntity
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
