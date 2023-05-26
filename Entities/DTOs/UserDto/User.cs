using Entities.Concrete;

namespace Entities.DTOs.UserDto
{
    public class User : IBaseEntitiy
    {
        public string? Username { get; set; }
        public string? UserEmail { get; set; }
        public string? Password { get; set; }

    }
}
