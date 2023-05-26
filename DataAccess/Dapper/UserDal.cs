using Dapper;
using Entities.DTOs.UserDto;
using MySql.Data.MySqlClient;

namespace DataAccess.Dapper
{
    public interface IUserDal : IBaseRepository<User>
    {
        Task<bool> UserExists(string userName);
        Task<User> Login(string userName, string password);

    }
    public class UserDal : IUserDal
    {
        public async Task<int> Add(User entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO Users (Username, UserEmail,Password) VALUES (@Username, @UserEmail,@Password);", new { Username = entity.Username, UserEmail = entity.UserEmail, Password =entity.Password});

                return result;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("DELETE FROM Users WHERE ID = @Id ", new { Id = id });

                return result;
            }
        }

        public async Task<User> Get(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE  ID = @Id ", new { Id = id });

                return result;
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {

            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<User>("SELECT * FROM Users");

                return result;
            }
        }

        public async Task<User> Login(string userName, string password)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Username=@Username AND Password=@Password", new { Username = userName, Password = password });

                return result;
            }
        }

        public async Task<int> Update(User entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("UPDATE Users  SET Username@Username, UserEmail=@UserEmail Password=@Password WHERE Id = @Id ;", new { Id = entity.Id, Username = entity.Username, UserEmail = entity.UserEmail, Password=entity.Password });

                return result;
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Username=@Username", new { Username = userName });

                return result == null ? false : true;
            }
        }
    }
}
