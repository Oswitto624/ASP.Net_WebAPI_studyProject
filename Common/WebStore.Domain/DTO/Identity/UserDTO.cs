

using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Domain.DTO.Identity;

public abstract class UserDTO
{
    public User User { get; set; }
}

public class AddLoginDTO : UserDTO
{
    public UserLoginInfo UserLoginInfo { get; set; } = null!;
}

public class PasswordHashDTO : UserDTO
{
    public string Hash { get; set; } = null!;
}

public class SetLockoutDTO : UserDTO
{
    public DateTimeOffset? LockoutEnd { get; set; }
}