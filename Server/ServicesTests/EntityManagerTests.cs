using ExamManager.Models;
using ExamManager.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace ServicesTests
{
    public class EntityManagerTests
    {
        EntityManager _entityManager = new EntityManager();
        static SecurityService _securityService = new SecurityService();

        [Theory]
        [MemberData(nameof(UserFieldsData))]
        public void UserFields_AreCopied(User user, Property[] properties, User resultUser)
        {            
            var userCopyManager = _entityManager.CopyInto(user);
            foreach(var property in properties)
            {
                userCopyManager.Property(property.Name, property.Value);
            }

            user = userCopyManager.GetResult();

            Assert.Equal(user, resultUser, new UsersComparer());
        }

        private static IEnumerable<object[]> UserFieldsData()
        {
            yield return new object[]
            {
                new User
                {
                    FirstName = "Иван",
                    LastName = "Петров",
                    Login = "ivan",
                    PasswordHash = _securityService.Encrypt("123"),
                    Role = UserRole.STUDENT
                },
                new Property[]
                {
                    new Property
                    {
                        Name = nameof(User.FirstName),
                        Value = "Петр"
                    },
                    new Property
                    {
                        Name = nameof(User.Login),
                        Value = "petr"
                    }
                },
                new User
                {
                    FirstName = "Петр",
                    LastName = "Петров",
                    Login = "petr",
                    PasswordHash = _securityService.Encrypt("123"),
                    Role = UserRole.STUDENT
                }
            };

            yield return new object[]
            {
                new User
                {
                    FirstName = "Иван",
                    LastName = "Петров",
                    Login = "ivan",
                    PasswordHash = _securityService.Encrypt("123"),
                    Role = UserRole.STUDENT
                },
                new Property[]
                {
                    new Property
                    {
                        Name = nameof(User.FirstName),
                        Value = "Петр"
                    },
                    new Property
                    {
                        Name = nameof(User.LastName),
                        Value = "Иванов"
                    },
                    new Property
                    {
                        Name = nameof(User.PasswordHash),
                        Value = _securityService.Encrypt("321")
                    }
                },
                new User
                {
                    FirstName = "Петр",
                    LastName = "Иванов",
                    Login = "petr",
                    PasswordHash = _securityService.Encrypt("321"),
                    Role = UserRole.STUDENT
                }
            };
        }
    }

    public class UsersComparer : IEqualityComparer<User>
    {
        public bool Equals(User? x, User? y)
        {
            var firstName = x.FirstName == y.FirstName;
            var lastName = x.LastName == y.LastName;

            return firstName && lastName;
        }

        public int GetHashCode([DisallowNull] User obj)
        {
            return obj.GetHashCode();
        }
    }
}