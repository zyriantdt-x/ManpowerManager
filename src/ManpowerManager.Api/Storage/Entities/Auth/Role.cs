namespace ManpowerManager.Api.Storage.Entities.Auth;

public class Role {
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public ICollection<UserRole> UserRoles { get; set; } = null!;
}
