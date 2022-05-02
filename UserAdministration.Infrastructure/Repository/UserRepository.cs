using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAdministration.Application.Interfaces;
using UserAdministration.Core.Entities;
using static UserAdministration.Application.Common.Enum;

namespace UserAdministration.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration configuration;
        public UserRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public int Add(User entity)
        {
            try
            {
                var userEntity = GetByIdAsync(entity.Id);

                if (userEntity.Exception == null && userEntity.Result == null)
                    userEntity = GetByUsernameAsync(entity.Username, entity.State);
                else
                    throw new Exception(message: ValidateMessages.UserAlreadyExistsId);

                if (userEntity.Exception == null && userEntity.Result == null)
                    userEntity = GetByEmailAsync(entity.Email, entity.State);
                else
                    throw new Exception(message: ValidateMessages.UserAlreadyExistsUsername);

                if (userEntity.Result == null && userEntity.Exception == null)
                {
                    #region Creation User
                    var sql = "INSERT into Users (username, password, email, creation, state) VALUES (@Username,@Password,@Email,@Creation, @State)";
                    using (var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();
                        connection.Execute("START TRANSACTION;");
                        var result = connection.Execute(sql, entity);
                        if (entity.Roles.Count > 0 && result != 0)
                        {
                            entity.Id = Convert.ToInt32(connection.ExecuteScalar("SELECT LAST_INSERT_ID();").ToString());
                            foreach (Roles role in entity.Roles)
                            {
                                UserRole entityUserRole = new()
                                {
                                    Id_role = role.Id,
                                    Id_user = entity.Id,
                                    Creation = DateTime.Now,
                                    State = RowStates.AvailableState
                                };
                                sql = "INSERT into user_role (id_user, id_role, creation, state) VALUES (@IdUser,@IdRole,@Creation,@State)";
                                result = connection.Execute(sql, new { IdUser = entityUserRole.Id_user, IdRole = entityUserRole.Id_role, entityUserRole.Creation, entityUserRole.State});
                            }
                        }
                        connection.Execute("COMMIT;");
                        connection.Close();
                    };
                    #endregion
                }
                else
                {
                    throw new Exception(message: ValidateMessages.UserAlreadyExistsEmail);
                }
                return entity.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(User entityRequest)
        {
            try
            {
                var userEntity = GetByIdAsync(entityRequest.Id);

                if (userEntity.Exception == null && userEntity.Result == null)
                    userEntity = GetByUsernameAsync(entityRequest.Username, RowStates.AvailableState);

                if (userEntity.Exception == null && userEntity.Result == null)
                    userEntity = GetByEmailAsync(entityRequest.Email, RowStates.AvailableState);

                if (userEntity.Result != null && userEntity.Exception == null)
                {
                    entityRequest.State = userEntity.Result.State;
                    entityRequest.Id = userEntity.Result.Id;

                    if (userEntity.Result.State == RowStates.NotAvailableState)
                        throw new Exception(message: ValidateMessages.UserDeleted);

                    var sql = "UPDATE Users SET username = @Username, email = @Email, password = @Password, modification = @Modification ";
                    sql += "WHERE id = @Id and state=@State";
                    using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
                    connection.Open();
                    connection.Execute("START TRANSACTION;");
                    var result = connection.Execute(sql, entityRequest);
                    List<UserRole> rolesExisting = GetRolesByUserIdAsync(entityRequest.Id);

                    var rolesToDelete = rolesExisting.Where(p => entityRequest.Roles.All(p2 => p2.Id != p.Id_role)).ToList();
                    foreach(UserRole roleDel in rolesToDelete)
                    {
                        sql = "UPDATE user_role SET modification = @Modification, state =@State WHERE id= @Id";
                        result = connection.Execute(sql, new { Modification = entityRequest.Modification, State = RowStates.NotAvailableState, Id = roleDel.Id });
                    }

                    var rolesToAdd = entityRequest.Roles.Where(p => rolesExisting.All(p2 => p2.Id_role != p.Id)).ToList();
                    foreach (Roles roleInsert in rolesToAdd)
                    {
                        entityRequest.Creation = DateTime.Now;
                        sql = "INSERT into user_role (id_user, id_role, creation, state) VALUES (@IdUser,@IdRole,@Creation,@State)";
                        result = connection.Execute(sql, new { IdUser = entityRequest.Id, IdRole = roleInsert.Id, Creation = entityRequest.Creation, State = RowStates.AvailableState });
                    }

                    connection.Execute("COMMIT;");
                    connection.Close();
                }
                else
                {
                    throw new Exception(message: ValidateMessages.UserNoExists);
                }
                return userEntity.Result.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int DeleteUser(User entity)
        {
            try
            {
                var oldState = RowStates.AvailableState;

                var userEntity = GetByIdStateAsync(entity.Id, oldState);

                if (userEntity.Exception == null && userEntity.Result == null)
                    userEntity = GetByUsernameAsync(entity.Username, RowStates.AvailableState);

                if (userEntity.Exception == null && userEntity.Result == null)
                    userEntity = GetByEmailAsync(entity.Email, RowStates.AvailableState);

                if (userEntity.Result != null && userEntity.Exception == null)
                {
                    entity.Id = userEntity.Result.Id;

                    var sql = "UPDATE Users SET modification = @Modification, state =@State WHERE id = @Id and state=@StateOld";
                    using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
                    connection.Open();
                    var result = connection.Execute(sql, new { entity.Modification, entity.State, entity.Id, StateOld = oldState });
                    sql = "UPDATE user_role SET modification = @Modification, state =@State WHERE id_user = @Id and state=@StateOld";
                    result = connection.Execute(sql, new { entity.Modification, entity.State, entity.Id, StateOld = oldState });
                    connection.Close();
                }
                else
                {
                    throw new Exception(message: ValidateMessages.UserNoExists);
                }
                return userEntity.Result.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<User> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Users WHERE id = @Id";
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            var result = await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
            connection.Close();
            return result;
        }

        private async Task<User> GetByIdStateAsync(int id, string state)
        {
            var sql = "SELECT * FROM Users WHERE id = @Id and state = @State";
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            var result = await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id, State = state});
            connection.Close();
            return result;
        }

        private async Task<User> GetByUsernameAsync(string username, string state)
        {
            var sql = "SELECT * FROM Users WHERE username = @Username and state = @State";
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            var result = await connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username, State = state });
            connection.Close();
            return result;
        }

        private async Task<User> GetByEmailAsync(string email, string state)
        {
            var sql = "SELECT * FROM Users WHERE email = @Email and state = @State";
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            var result = await connection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email, State = state });
            connection.Close();
            return result;
        }

        private List<UserRole> GetRolesByUserIdAsync(int userId)
        {
            var sql = "SELECT * FROM user_role WHERE id_user = @IdUser and state = @State";
            using (var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = connection.Query<UserRole>(sql, new { IdUser = userId , State = RowStates.AvailableState});
                return result.ToList();
            }
        }

        public List<User> GetUsersAvailable(string state)
        {
            var sql = "SELECT * FROM Users WHERE state = @State";
            using (var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = connection.Query<User>(sql, new { State = RowStates.AvailableState });
                return result.ToList();
            }
        }
    }
}
