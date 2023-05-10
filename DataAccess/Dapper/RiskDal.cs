using Dapper;
using Entities.Concrete;
using MySql.Data.MySqlClient;

namespace DataAccess.Dapper
{
    public interface IRiskDal : IBaseRepository<Risk>
    {
    }
    public class RiskDal : IRiskDal
    {
        public async Task<int> Add(Risk entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO Risks (ProjeId, RiskTanimi, RiskKategorisi, Olasilik, Etki, RiskSkoru, RiskOnceligi, RiskDurumu) VALUES (@ProjeId, @RiskTanimi, @RiskKategorisi, @Olasilik, @Etki, @RiskSkoru, @RiskOnceligi, @RiskDurumu);", new
                {
                    ProjeId = entity.ProjeId,
                    RiskTanimi = entity.RiskTanimi,
                    RiskKategorisi = entity.RiskKategorisi,
                    Olasilik = entity.Olasilik,
                    Etki = entity.Etki,
                    RiskSkoru = entity.RiskSkoru,
                    RiskOnceligi = entity.RiskOnceligi,
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

        public async Task<int> Update(Risk entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync(@"UPDATE Risks SET ProjeId=@ProjeId,RiskTanimi=@RiskTanimi,RiskKategorisi=@RiskKategorisi,             Olasilik=@Olasilik,Etki=@Etki,RiskSkoru=@RiskSkoru,RiskOnceligi=@RiskOnceligi,RiskDurumu=@RiskDurumu WHERE ID = @Id;", new
                {
                    Id= entity.Id,
                    ProjeId = entity.ProjeId,
                    RiskTanimi = entity.RiskTanimi,
                    RiskKategorisi = entity.RiskKategorisi,
                    Olasilik = entity.Olasilik,
                    Etki = entity.Etki,
                    RiskSkoru = entity.RiskSkoru,
                    RiskOnceligi = entity.RiskOnceligi,
                    RiskDurumu = entity.RiskDurumu
                });
                return result;
            }
        }
    }
}
