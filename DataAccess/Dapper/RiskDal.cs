using Dapper;
using Entities.Concrete;
using MySql.Data.MySqlClient;

namespace DataAccess.Dapper
{
    public interface IRiskDal : IBaseRepository<Risk>
    {
        Task<IEnumerable<Risk>> GetRiskByProjectId(int projectId);  
    }
    public class RiskDal : IRiskDal
    {
        public async Task<int> Add(Risk entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO Risks (ProjeId, RiskTanimi, RiskDurumu) VALUES (@ProjeId, @RiskTanimi, @RiskDurumu);", new
                {
                    ProjeId = entity.ProjeId,
                    RiskTanimi = entity.RiskTanimi,             
                    RiskDurumu = entity.RiskDurumu
                });
                return result;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("DELETE FROM Risks WHERE ID = @Id "
                    , new { Id = id });
                return result;
            }
        }

        public async Task<Risk> Get(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<Risk>("SELECT * FROM Risks WHERE ID = @Id "
                    , new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<Risk>> GetAll()
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<Risk>("SELECT * FROM Risks");
                return result;
            }
        }

        public async Task<IEnumerable<Risk>> GetRiskByProjectId(int projectId)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<Risk>("SELECT * FROM Risks  WHERE ProjeId = @ProjeId ", new { ProjeId = projectId });

                return result;
            }
        }

        public async Task<int> Update(Risk entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync(@"UPDATE Risks SET ProjeId=@ProjeId,RiskTanimi=@RiskTanimi,RiskDurumu=@RiskDurumu WHERE ID = @Id;", new
                {
                    Id= entity.Id,
                    ProjeId = entity.ProjeId,
                    RiskTanimi = entity.RiskTanimi,
                    RiskDurumu = entity.RiskDurumu
                });
                return result;
            }
        }
    }
}
