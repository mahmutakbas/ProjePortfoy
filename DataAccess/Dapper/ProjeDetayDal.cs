using Dapper;
using Entities.Concrete;
using MySql.Data.MySqlClient;

namespace DataAccess.Dapper
{
    public interface IProjeDetayDal : IBaseRepository<ProjeDetay>
    {
    }
    public class ProjeDetayDal : IProjeDetayDal
    {
        public async Task<int> Add(ProjeDetay entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO ProjeDetays (ProjeId, KaynakId, KaynakMiktari) VALUES (@ProjeId, @KaynakId, @KaynakMiktari);", new { ProjeId = entity.ProjeId, KaynakId = entity.KaynakId, KaynakMiktari = entity.KaynakId });
                return result;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("DELETE FROM ProjeDetays WHERE ID = @Id", new { Id = id });
                return result;
            }
        }

        public async Task<ProjeDetay> Get(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<ProjeDetay>("SELECT * FROM ProjeDetays WHERE ID = @Id", new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<ProjeDetay>> GetAll()
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<ProjeDetay>("SELECT * FROM ProjeDetays");
                return result;
            }
        }

        public async Task<int> Update(ProjeDetay entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("UPDATE ProjeDetays SET ProjeId=@ProjeId, KaynakId=@KaynakId, KaynakMiktari=@KaynakMiktari WHERE ID =@Id;", new { Id = entity.Id, ProjeId = entity.ProjeId, KaynakId = entity.KaynakId, KaynakMiktari = entity.KaynakId });
                return result;
            }
        }
    }
}
