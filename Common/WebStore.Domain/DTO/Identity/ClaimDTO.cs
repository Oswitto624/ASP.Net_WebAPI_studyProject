using System.Security.Claims;

namespace WebStore.Domain.DTO.Identity;

public class ClaimDTO : UserDTO
{
    public IEnumerable<Claim> Claims { get; set; } = null!;
}

public class ReplaceClaimDTO : UserDTO
{
    public Claim Claim { get; set; } = null!;
    public Claim NewClaim { get; set; } = null!;
}