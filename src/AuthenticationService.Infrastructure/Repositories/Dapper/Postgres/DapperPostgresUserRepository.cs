using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Core.Domain.User;
using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;
using AuthenticationService.Infrastructure.Configurations.Repository;
using Dapper;

namespace AuthenticationService.Infrastructure.Repositories.Dapper.Postgres
{
    public class DapperPostgresUserRepository : DapperRepositoryBase, IUserRepository
    {
        private readonly string _usersTableName;

        public DapperPostgresUserRepository(
            PostgresConfiguration postgresConfiguration,
            IDbTransaction dbTransaction
        ) : base(dbTransaction)
        {
            _usersTableName = postgresConfiguration.UsersTableName;
        }

        public async Task<UserEntity> GetByEmailAsync(string email)
        {
            string findUserByEmailQuery = $@"SELECT * FROM ""{_usersTableName}"" WHERE ""Email"" = '{email}';";

            return (await DbConnection.QueryAsync<dynamic>(findUserByEmailQuery, transaction: DbTransaction))
                                      .Select(convertDatabaseUserToUserEntity)
                                      .FirstOrDefault();
  
        }

        public async Task<UserEntity> GetByUserNameAsync(string userName)
        {
            string findUserByUserNameQuery = $@"SELECT * FROM ""{_usersTableName}"" WHERE ""UserName"" = '{userName}';";

            return (await DbConnection.QueryAsync<dynamic>(findUserByUserNameQuery, transaction: DbTransaction))
                                      .Select(convertDatabaseUserToUserEntity)
                                      .FirstOrDefault();
        }

        public async Task<UserEntity> GetByIdAsync(Guid id)
        {
            string getUserByIdQuery = $@"SELECT * FROM ""{_usersTableName}"" WHERE ""Id"" = '{id}';";

            return (await DbConnection.QueryAsync<dynamic>(getUserByIdQuery, transaction: DbTransaction))
                                      .Select(convertDatabaseUserToUserEntity)
                                      .FirstOrDefault();
        }

        public async Task<IEnumerable<UserEntity>> GetAllAsync()
        {
            string getAllUsersQuery = $@"SELECT * FROM ""{_usersTableName}"";";

            return (await DbConnection.QueryAsync<dynamic>(getAllUsersQuery, transaction: DbTransaction))
                                      .Select(convertDatabaseUserToUserEntity)
                                      .AsQueryable();

        }

        public async Task AddAsync(UserEntity user)
        {
            await addUsersAsync(new[] { user });
        }

        public async Task AddRangeAsync(IEnumerable<UserEntity> users)
        {
            await addUsersAsync(users);
        }

        public async Task RemoveAsync(UserEntity user)
        {
            await removeUsersAsync(new[] { user });
        }

        public async Task RemoveRangeAsync(IEnumerable<UserEntity> users)
        {
            await removeUsersAsync(users);
        }

        public async Task UpdateAsync(UserEntity newUser)
        {
            string updateUserQuery = $@"UPDATE ""{_usersTableName}"" " + 
                                        @"SET ""UserName"" = @UserName, " +
                                            @"""Email"" = @Email, " +
                                            @"""Gender"" = @Gender', " +
                                            @"""Role"" = @Role, " + 
                                            @"""DateOfBirth"" = @DateOfBirth" +
                                        @"WHERE ""Id"" = @Id;";

            await DbConnection.ExecuteAsync(updateUserQuery, newUser, transaction: DbTransaction);
        }

        private async Task addUsersAsync(IEnumerable<UserEntity> users)
        {
            string addUserQuery = $@"INSERT INTO ""{_usersTableName}"" " +
            @"(""UserName"", ""Email"", ""Gender"", ""Role"", ""DateOfBirth"", ""Password"", ""LastLoginDate"", ""CreationDate"") " + 
            "VALUES (@UserName, @Email, @Gender, @Role, @DateOfBirth, @Password, @LastLoginDate, @CreationDate);";

            foreach (var user in users) {
                await DbConnection.ExecuteAsync(addUserQuery,
                                                convertUserEntityToDatabaseUser(user),
                                                transaction: DbTransaction);
            }
        } 

        private async Task removeUsersAsync(IEnumerable<UserEntity> users)
        {
            Guid[] userIds = new Guid[users.Count()];

            for (int idx = 0; idx < users.Count(); idx++) {
                userIds[idx] = users.ElementAt(idx).Id;
            }

            string userIdsJoinedByComma = String.Join(',', userIds);

            string deleteUsersQuery = $@"DELETE FROM ""{_usersTableName}"" WHERE ""Id"" in ({userIdsJoinedByComma});";

            await DbConnection.ExecuteAsync(deleteUsersQuery, transaction: DbTransaction);
        }

        private UserEntity convertDatabaseUserToUserEntity(dynamic dbUser)
        {
            return new UserEntity() {
                UserName = dbUser.UserName, 
                Email = dbUser.Email,
                Gender = (GenderTypes)dbUser.Gender,
                Role = (RoleTypes)dbUser.Role,
                DateOfBirth = DateTime.Parse(dbUser.DateOfBirth.ToString()),
                LastLoginDate = dbUser.LastLoginDate == null ? null : DateTime.Parse(dbUser.LastLoginDate.ToString()),
                CreationDate = DateTime.Parse(dbUser.CreationDate.ToString()),
                Password = new Password() {
                    Type = PasswordTypes.Hashed,
                    Value = dbUser.Password
                    }
                };
        }  
        private object convertUserEntityToDatabaseUser(UserEntity user)
        {
            return new {
                UserName = user.UserName, 
                Email = user.Email,
                Gender = (int)user.Gender,
                Role = (int)user.Role,
                DateOfBirth = user.DateOfBirth,
                LastLoginDate = user.LastLoginDate,
                CreationDate = user.CreationDate,
                Password = user.Password.Value,
                };
        }
    }
}