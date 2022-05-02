using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using UserAdministration.Application;
using UserAdministration.Application.DTO;
using UserAdministration.Infrastructure.Repository;
using Xunit;

namespace UserAdministration.Testing
{
    public class UnitTest1
    {
        private UserUseCase _unitUnderTesting = null;
        public UnitTest1()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            UserRepository userRepository = new UserRepository(config);

            if (_unitUnderTesting == null)
            {
                _unitUnderTesting = new UserUseCase(userRepository);
            }
        }

        [Fact]
        public void CreateUserAdmin()
        {
            var userRequest = new UserRequest
            {
                Username = "Test" + RandomString(10),
                Password = RandomString(8),
                Email = RandomString(10) + "@gmail.com"
            };

            userRequest.Roles = new List<RoleRequest>();
            userRequest.Roles.Add(new RoleRequest
            {
                Id = 1,
                Name = "admin"
            });

            var result = _unitUnderTesting.Add(userRequest);

            Assert.True(result.Id > 0, "User created");
        }

        [Fact]
        public void UpdateUserAdmin()
        { 
            List<UsersTotalResponse> usersAvailables = _unitUnderTesting.GetUsersAvailable();
            var random = new Random();
            int index = random.Next(usersAvailables.Count);

            var userRequest = new UserUpdateRequest
            {
                Id = usersAvailables.ElementAtOrDefault(index).Id,
                Username = "New Name"+index,
                Email = "New Name" + index + "@hotmail.com",
                Password = "XXXX66666"
            };

            userRequest.Roles = new List<RoleRequest>();
            userRequest.Roles.Add(new RoleRequest
            {
                Id = 1,
                Name = "admin"
            });

            var result = _unitUnderTesting.Update(userRequest);

            Assert.True(result.Id > 0, "User up to date");
        }

        [Fact]
        public void UpdateUserRoleUser()
        {
            List<UsersTotalResponse> usersAvailables = _unitUnderTesting.GetUsersAvailable();
            var random = new Random();
            int index = random.Next(usersAvailables.Count);

            var userRequest = new UserUpdateRequest
            {
                Id = usersAvailables.ElementAtOrDefault(index).Id,
                Username = "New Name" + index,
                Email = "New Name" + index + "@hotmail.com",
                Password = "XXXX66666"
            };

            userRequest.Roles = new List<RoleRequest>();
            userRequest.Roles.Add(new RoleRequest
            {
                Id = 2,
                Name = "user"
            });

            var result = _unitUnderTesting.Update(userRequest);

            Assert.True(result.Id > 0, "User up to date");
        }

        [Fact]
        public void UpdateUserByEmail()
        {
            List<UsersTotalResponse> usersAvailables = _unitUnderTesting.GetUsersAvailable();
            var random = new Random();
            int index = random.Next(usersAvailables.Count);

            var userRequest = new UserUpdateRequest
            {
                Username = "New Name By Mail" + index,
                Email = usersAvailables.ElementAtOrDefault(index).Email,
                Password = "XXXX66666"
            };

            userRequest.Roles = new List<RoleRequest>();
            userRequest.Roles.Add(new RoleRequest
            {
                Id = 2,
                Name = "user"
            });

            var result = _unitUnderTesting.Update(userRequest);

            Assert.True(result.Id > 0, "User up to date");
        }

        [Fact]
        public void DeleteUser()
        {
            List<UsersTotalResponse> usersAvailables = _unitUnderTesting.GetUsersAvailable();
            var random = new Random();
            int index = random.Next(usersAvailables.Count);

            var userRequest = new UserUpdateRequest
            {
                Id = usersAvailables.ElementAtOrDefault(index).Id
            };

            var result = _unitUnderTesting.Delete(userRequest);

            Assert.True(result.Id > 0, "User up to date");
        }

        private static readonly Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
