﻿namespace ManpowerManager.Api.Storage.Entities.Auth;

public class User {
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
}
