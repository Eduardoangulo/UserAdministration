using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UserAdministration.Application.DTO;
using UserAdministration.Application.Exceptions;
using UserAdministration.Application.Interfaces;
using UserAdministration.Core.Entities;
using static UserAdministration.Application.Common.Enum;

namespace UserAdministration.Application
{
    public class UserUseCase : PasswordHasher, IUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public UserUseCase()
        {
        }

        public UserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserResponse Add(UserRequest user)
        {
            try
            {
                UserUseCase userUseCase = new UserUseCase();
                user.Password = userUseCase.HashPassword(user.Password);

                User userEntity = new()
                {
                    Username = user.Username,
                    Password = user.Password,
                    Email = user.Email,
                    Creation = DateTime.Now,
                    State = RowStates.AvailableState
                };

                userEntity.Roles = new List<Roles>();

                foreach (RoleRequest roleRequest in user.Roles)
                {
                    userEntity.Roles.Add(new Roles {
                        Id = roleRequest.Id,
                        Name = roleRequest.Name
                    });
                }

                return new UserResponse { Id = _userRepository.Add(userEntity) };
            }
            catch (Exception e)
            {
                HttpResponseException ex = new HttpResponseException
                {
                    Value = e.Message,
                    Status = (int)HttpStatusCode.BadRequest
                };
                throw ex;
            }
        }

        public UserResponse Update(UserUpdateRequest user)
        {
            try
            {
                UserUseCase userUseCase = new UserUseCase();
                user.Password = userUseCase.HashPassword(user.Password);

                User userEntity = new()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = user.Password,
                    Email = user.Email,
                    Modification = DateTime.Now,
                    State = RowStates.AvailableState
                };

                userEntity.Roles = new List<Roles>();

                foreach (RoleRequest roleRequest in user.Roles)
                {
                    userEntity.Roles.Add(new Roles
                    {
                        Id = roleRequest.Id,
                        Name = roleRequest.Name
                    });
                }

                return new UserResponse { Id = _userRepository.Update(userEntity) };
            }
            catch (Exception e)
            {
                HttpResponseException ex = new HttpResponseException
                {
                    Value = e.Message,
                    Status = (int)HttpStatusCode.BadRequest
                };
                throw ex;
            }
        }

        public UserResponse Delete(UserUpdateRequest user)
        {
            try
            {
                User userEntity = new()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Modification = DateTime.Now,
                    State = RowStates.NotAvailableState
                };

                return new UserResponse { Id = _userRepository.DeleteUser(userEntity) };
            }
            catch (Exception e)
            {
                HttpResponseException ex = new HttpResponseException
                {
                    Value = e.Message,
                    Status = (int)HttpStatusCode.BadRequest
                };
                throw ex;
            }
        }
        public List<UsersTotalResponse> GetUsersAvailable()
        {
            try
            {
                List<UsersTotalResponse> listTotal = new List<UsersTotalResponse>();
                var resultList = _userRepository.GetUsersAvailable(RowStates.NotAvailableState);
                foreach(User user in resultList)
                {
                    listTotal.Add(new UsersTotalResponse
                    {
                        Username = user.Username,
                        Id = user.Id,
                        Password = user.Password,
                        Email = user.Email,
                        Creation = user.Creation,
                        Modification = user.Modification,
                        State = user.State
                    });
                }
                return listTotal;
            }
            catch (Exception e)
            {
                HttpResponseException ex = new HttpResponseException
                {
                    Value = e.Message,
                    Status = (int)HttpStatusCode.BadRequest
                };
                throw ex;
            }
        }
    }
}
