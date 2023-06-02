using Dapper;
using Entities.Concrete;
using MySql.Data.MySqlClient;

namespace DataAccess.Dapper
{
    public interface IProjeKPIDal : IBaseRepository<ProjeKPI>
    {
        Task<IEnumerable<ProjeKPI>> GetByProjectId(int projeId);
    }
    public class ProjeKPIDal : IProjeKPIDal
    {
        public async Task<int> Add(ProjeKPI entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO ProjeKPIs (ProjeId, Name,Goal) VALUES (@ProjeId, @Name,@Goal)", new { ProjeId = entity.ProjeId, Name = entity.Name, Goal = entity.Goal });

                return result;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("DELETE FROM ProjeKPIs WHERE ID = @Id", new { Id = id });

                return result;
            }
        }

        public async Task<ProjeKPI> Get(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<ProjeKPI>("SELECT * FROM ProjeKPIs WHERE ID = @Id", new { Id = id });

                return result;
            }
        }

        public async Task<IEnumerable<ProjeKPI>> GetAll()
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<ProjeKPI>("SELECT * FROM ProjeKPIs ");

                return result;
            }
        }

        public async Task<IEnumerable<ProjeKPI>> GetByProjectId(int projeId)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<ProjeKPI>("SELECT * FROM ProjeKPIs WHERE ProjeId = @ProjeId", new { ProjeId = projeId });

                return result;
            }
        }

        public async Task<int> Update(ProjeKPI entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("UPDATE ProjeKPIs SET  ProjeId=@ProjeId, Name=@Name,Goal=@Goal  WHERE ID = @Id ", new { Id = entity.Id, ProjeId = entity.ProjeId, Name = entity.Name, Goal = entity.Goal });

                return result;
            }
        }
    }
}
