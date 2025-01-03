using ManpowerManager.Api.Storage;
using ManpowerManager.Api.Storage.Entities.Auth;
using ManpowerManager.Shared.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ManpowerManager.Api.Controllers;
[ApiController]
[Route( "[controller]" )]
public class AuthController : ControllerBase {
    private readonly MpStorage storage;

    private readonly string signing_key;
    private readonly string issuer;
    private readonly string audience;

    public AuthController( MpStorage storage, IConfiguration configuration ) {
        this.storage = storage;

        this.signing_key = configuration[ "Jwt:Key" ] ?? throw new Exception();
        this.issuer = configuration[ "Jwt:Issuer" ] ?? throw new Exception();
        this.audience = configuration[ "Jwt:Issuer" ] ?? throw new Exception();
    }

    [HttpGet]
    public async Task<IActionResult> Get() {
        await Task.Run( () => { } );
        return this.Ok( "hi" );
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register( [FromBody] RegisterRequest req ) {
        // Hash password in a real application
        this.storage.Users.Add( new() { 
            Id = Guid.NewGuid().ToString(),
            Username = req.Username,
            Password = req.Password
        } );

        await this.storage.SaveChangesAsync();

        /*foreach( var roleName in roles ) {
            var role = await _context.Roles.FirstOrDefaultAsync( r => r.Name == roleName );
            if( role != null ) {
                _context.UserRoles.Add( new UserRole { UserId = user.Id, RoleId = role.Id } );
            }
        }
        await _context.SaveChangesAsync();*/

        return this.Ok();
    }

    [HttpPost( "login" )]
    public async Task<IActionResult> Login( [FromBody] LoginRequest req ) {
        User? user = await this.storage.Users
            .Include( u => u.UserRoles )
                .ThenInclude( ur => ur.Role )
            .FirstOrDefaultAsync( u => u.Username == req.Username && u.Password == req.Password ); // Hash password comparison in real app

        if( user is null )
            return this.Unauthorized();

        string token = this.GenerateJwtToken( user );
        return this.Ok( new { token } );
    }

    private string GenerateJwtToken( User user ) {
        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange( user.UserRoles.Select( ur => new Claim( ClaimTypes.Role, ur.Role.Name ) ) );

        SymmetricSecurityKey key = new( Encoding.UTF8.GetBytes( this.signing_key ) );
        SigningCredentials creds = new( key, SecurityAlgorithms.HmacSha256 );

        JwtSecurityToken token = new(
            issuer: this.issuer,
            audience: this.audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes( 30 ),
            signingCredentials: creds );

        return new JwtSecurityTokenHandler().WriteToken( token );
    }
}
