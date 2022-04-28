using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserAdministration.Application.DTO;
using UserAdministration.Application.Interfaces;
using UserAdministration.Core.Entities;

namespace UserAdministration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserUseCase _user;

        public UserController(IUserUseCase user)
        {
            _user = user;
        }

        [HttpPost("addUser")]
        public UserResponse Add(UserRequest user)
        {
            return _user.Add(user);
        }

        [HttpPost("updateUser")]
        public UserResponse Update(UserUpdateRequest user)
        {
            return _user.Update(user);
        }

        [HttpPost("deleteUser")]
        public UserResponse Delete(UserUpdateRequest user)
        {
            return _user.Delete(user);
        }

    }
}
