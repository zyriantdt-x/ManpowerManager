using ManpowerManager.Api.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<MpStorage>( options =>
    options.UseSqlite( builder.Configuration.GetConnectionString( "DefaultConnection" ) ) );

builder.Services.AddAuthentication( options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
} )
.AddJwtBearer( options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration[ "Jwt:Issuer" ],
        ValidAudience = builder.Configuration[ "Jwt:Audience" ],
        IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( builder.Configuration[ "Jwt:Key" ]! ) )
    };
} );

builder.Services.AddAuthorization( options => {
    options.AddPolicy( "Admin", policy => policy.RequireRole( "Admin" ) );
    options.AddPolicy( "User", policy => policy.RequireRole( "User" ) );
} );

// build the app
WebApplication app = builder.Build();

// seed storage
using( IServiceScope scope = app.Services.CreateScope() ) {
    MpStorage storage = scope.ServiceProvider.GetRequiredService<MpStorage>();
    storage.Database.Migrate();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
