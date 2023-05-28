using Dapper;
using Entities.Concrete;
using MySql.Data.MySqlClient;

namespace DataAccess.Dapper
{
    public interface IKaynakDal : IBaseRepository<Kaynak>
    {
        Task<bool> IsExist(Kaynak entity);
    }
    public class KaynakDal : IKaynakDal
    {
        public async Task<int> Add(Kaynak entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO Kaynaks (KaynakAdi, KaynakMiktari,DepartmanId) VALUES (@KaynakAdi, @KaynakMiktari,@DepartmanId)", new { KaynakAdi = entity.KaynakAdi, KaynakMiktari = entity.KaynakMiktari, DepartmanId = entity.DepartmanId });
                return result;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("DELETE FROM Kaynaks WHERE ID = @Id"
                    , new { Id = id });
                return result;
            }
        }

        public async Task<Kaynak> Get(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<Kaynak>("SELECT * FROM Kaynaks WHERE ID = @Id"
                    , new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<Kaynak>> GetAll()
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<Kaynak>("SELECT * FROM Kaynaks");
                return result;
            }
        }

        public async Task<bool> IsExist(Kaynak entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<Kaynak>("SELECT * FROM Kaynaks WHERE KaynakAdi=@KaynakAdi AND ID <> @Id", new { KaynakAdi = entity.KaynakAdi, Id = entity.Id });

                return result == null ? false : true;
            }
        }

        public async Task<int> Update(Kaynak entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("UPDATE INTO Kaynaks (KaynakAdi, KaynakMiktari,DepartmanId) VALUES (@KaynakAdi, @KaynakMiktari,@DepartmanId)", new { KaynakAdi = entity.KaynakAdi, KaynakMiktari = entity.KaynakMiktari, DepartmanId = entity.DepartmanId });
                return result;
            }
        }
    }
}
