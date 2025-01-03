using ManpowerManager.Api.Storage.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ManpowerManager.Api.Storage;

public class MpStorage : DbContext {
    public MpStorage( DbContextOptions<MpStorage> options )
    : base( options ) {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating( ModelBuilder mb ) {
        mb.Entity<Role>().HasData(
            new Role { Id = Guid.NewGuid().ToString(), Name = "Admin" },
            new Role { Id = Guid.NewGuid().ToString(), Name = "User" }
        );

        mb.Entity<User>().HasData(
            new User { Id = Guid.NewGuid().ToString(), Username = "admin", Password = "123" },
            new User { Id = Guid.NewGuid().ToString(), Username = "user", Password = "123" }
        );

        mb.Entity<UserRole>()
            .HasKey( ur => new { ur.UserId, ur.RoleId } );

        mb.Entity<UserRole>()
            .HasOne( ur => ur.User )
            .WithMany( u => u.UserRoles )
            .HasForeignKey( ur => ur.UserId );

        mb.Entity<UserRole>()
            .HasOne( ur => ur.Role )
            .WithMany( r => r.UserRoles )
            .HasForeignKey( ur => ur.RoleId );
    }
}
