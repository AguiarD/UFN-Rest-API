using Microsoft.EntityFrameworkCore;
using WebApplicationAulaRestAPI.Models;

namespace WebApplicationAulaRestAPI.Data
{
    public class Contexto : DbContext
    {
        public DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
            opt.UseSqlServer(@"Data Source=localhost\MSSQLSERVER01; initial Catalog=RestAPI; User ID=sa; password=1234;language=Portuguese;Trusted_Connection=True");
        }
    }
}
