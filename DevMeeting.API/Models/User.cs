using System;

namespace DevMeeting.API.Models
{
    public class User
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        public User WithId(string id)
        {
            Id = id;
            return this;
        }
    }
}