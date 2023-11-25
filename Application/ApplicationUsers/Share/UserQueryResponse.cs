using Application.DTOs;

namespace Application.ApplicationUsers.Share;

public class UserQueryResponse : UserResponse
{
    public List<UserDto> Users { get; set; }
}