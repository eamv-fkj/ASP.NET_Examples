namespace SecureVcApp.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecureVcApp.Models;

public class AppDbContext : IdentityDbContext<AppUser> {
  
  public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
  { }

  // NEDARVES: public DbSet<TUser> Users; // TUser er her AppUser

  public DbSet<Order> Orders => Set<Order>();

  // NEDARVES: public DbSet<TRole> Roles;
  // NEDARVES: public DbSet<TUserRole> UserRoles;
}