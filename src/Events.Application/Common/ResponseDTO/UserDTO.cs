using Events.Application.Common.AutoMapper;
using Events.Domain.Entities;

namespace Events.Application.Common.ResponseDTO;

public class UserDTO : IMapFrom<User>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime DateOfBirth { get; set; }
    public bool isAdmin { get; set; }
}