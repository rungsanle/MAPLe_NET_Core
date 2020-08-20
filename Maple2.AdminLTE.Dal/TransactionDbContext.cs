using Maple2.AdminLTE.Bel;
using Microsoft.EntityFrameworkCore;


namespace Maple2.AdminLTE.Dal
{
    public class TransactionDbContext : DbContext
    {
        public TransactionDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<T_Arrival_Header> ArrivalHdr { get; set; }
        public DbSet<T_Arrival_Detail> ArrivalDtl { get; set; }
        public DbSet<T_Arrival_Detail_Sub> ArrivalDtlSub { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Arrival Hdr more specifications.
            modelBuilder.Entity<T_Arrival_Header>()
                .HasIndex(a => new { a.ArrivalNo, a.CompanyCode })
                .HasName("IX_ARRIVALANDCOMP")
                .IsUnique(true);

            modelBuilder.Query<T_Arrival_HeaderObj>();
            modelBuilder.Query<T_Arrival_DetailObj>();
            modelBuilder.Query<T_Arrival_Detail_SubObj>();
        }
    }
}
