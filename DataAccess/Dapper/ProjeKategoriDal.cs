using Dapper;
using Entities.Concrete;
using MySql.Data.MySqlClient;

namespace DataAccess.Dapper
{

    public interface IProjeKategoriDal : IBaseRepository<ProjeKategori>
    {
        Task<bool> IsExist(ProjeKategori entity);
    }
    public class ProjeKategoriDal : IProjeKategoriDal
    {
        public async Task<int> Add(ProjeKategori entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO ProjeKategoris (ProjeKategoriAdi) VALUES(@ProjeKategoriAdi);", new { ProjeKategoriAdi = entity.ProjeKategoriAdi });
                return result;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("DELETE FROM ProjeKategoris WHERE ID = @Id", new { Id = id });
                return result;
            }
        }

        public async Task<ProjeKategori> Get(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<ProjeKategori>("SELECT * FROM ProjeKategoris WHERE ID = @Id", new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<ProjeKategori>> GetAll()
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<ProjeKategori>("SELECT * FROM ProjeKategoris");
                return result;
            }
        }

        public async Task<bool> IsExist(ProjeKategori entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<ProjeKategori>("SELECT * FROM ProjeKategoris WHERE ProjeKategoriAdi=@ProjeKategoriAdi AND ID <> @Id", new { ProjeKategoriAdi = entity.ProjeKategoriAdi, Id = entity.Id });

                return result == null ? false : true;
            }
        }

        public async Task<int> Update(ProjeKategori entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("UPDATE ProjeKategoris SET ProjeKategoriAdi=@ProjeKategoriAdi WHERE ID = @Id;", new { Id = entity.Id, ProjeKategoriAdi = entity.ProjeKategoriAdi });
                return result;
            }
        }
    }
}
