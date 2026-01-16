using _2_Domain.Entities;
using _2_Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace _3_Infrastructure.Seed;

public class UserSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Email = "admin@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                Role =  Role.Admin,
            }
        );
    }
}