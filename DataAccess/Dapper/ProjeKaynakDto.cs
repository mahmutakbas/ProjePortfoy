using Dapper;
using Entities.Concrete;
using MySql.Data.MySqlClient;

namespace DataAccess.Dapper
{
    public interface IProjeKaynakDal : IBaseRepository<ProjeKaynak>
    {
        Task<bool> IsExist(ProjeKaynak entity);
        Task<IEnumerable<ProjeKaynak>> GetByProjectId(int projeId);
    }
    public class ProjeKaynakDto : IProjeKaynakDal
    {
        public async Task<int> Add(ProjeKaynak entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO ProjeKaynak (ProjeId, KaynakId, KaynakMiktari) VALUES (@ProjeId, @KaynakId, @KaynakMiktari)", new { ProjeId = entity.ProjeId, KaynakId = entity.KaynakId, KaynakMiktari = entity.KaynakMiktari });

                return result;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("DELETE FROM ProjeKaynak WHERE ID = @Id"
                    , new { Id = id });
                return result;
            }
        }

        public async Task<ProjeKaynak> Get(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<ProjeKaynak>("SELECT * FROM ProjeKaynak WHERE ID = @Id"
                    , new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<ProjeKaynak>> GetAll()
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<ProjeKaynak>("SELECT * FROM ProjeKaynak");
                return result;
            }
        }

        public async Task<IEnumerable<ProjeKaynak>> GetByProjectId(int projeId)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<ProjeKaynak>("SELECT * FROM ProjeKaynak WHERE ProjeId = @ProjeId"
                    , new { ProjeId = projeId });
                return result;
            }
        }

        public async Task<bool> IsExist(ProjeKaynak entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<Kaynak>("SELECT * FROM ProjeKaynak WHERE KaynakId=@KaynakId AND ID <> @Id", new { KaynakId = entity.KaynakId, Id = entity.Id });

                return result == null ? false : true;
            }
        }

        public async Task<int> Update(ProjeKaynak entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("UPDATE INTO Kaynaks (ProjeId, KaynakId, KaynakMiktari) VALUES (@ProjeId, @KaynakId, @KaynakMiktari)", new { ProjeId = entity.ProjeId, KaynakId = entity.KaynakId, KaynakMiktari = entity.KaynakMiktari });
                return result;
            }
        }
    }
}
