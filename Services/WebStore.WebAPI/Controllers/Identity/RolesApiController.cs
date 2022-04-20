using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;

namespace WebStore.WebAPI.Controllers.Identity;

[ApiController]
[Route(WebAPIAddresses.V1.Identity.Roles)]
public class RolesApiController : ControllerBase
{
    private readonly ILogger<RolesApiController> _Logger;
    private readonly RoleStore<Role> _RoleStore;
    public RolesApiController(WebStoreDB db, ILogger<RolesApiController> Logger)
    {
        _RoleStore = new(db);
        _Logger = Logger;
    }

    [HttpGet("all")]
    public async Task<IEnumerable<Role>> GetAll() => await _RoleStore.Roles.ToArrayAsync();

}
