using System;
using AuthenticationService.Core.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Infrastructure.Repositories.EFCore
{
    public class AuthenticationServiceDbContext : DbContext
    {   
        public virtual DbSet<UserEntity> Users { get; set; }

        public AuthenticationServiceDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .Property(user => user.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserEntity>()
                .HasIndex(user => user.Email).IsUnique();
            modelBuilder.Entity<UserEntity>()
                .Property(user => user.Email).IsRequired();

            modelBuilder.Entity<UserEntity>()
                .HasIndex(user => user.UserName).IsUnique();
            modelBuilder.Entity<UserEntity>()
                .Property(user => user.UserName).IsRequired();

            modelBuilder.Entity<UserEntity>()
                .Property(user => user.Password).IsRequired();
            modelBuilder.Entity<UserEntity>()
                .Property(user => user.Password).HasConversion(
                    v => v.Value,
                    v => new Password() {Type = PasswordTypes.Hashed, Value = v}
                );

            modelBuilder.Entity<UserEntity>()
                .Property(user => user.Role).HasConversion<int>();
                
            modelBuilder.Entity<UserEntity>()
                .Property(user => user.Gender).HasConversion<int>();
        }
    }
}