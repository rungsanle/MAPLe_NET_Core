using Maple2.AdminLTE.Bel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Maple2.AdminLTE.Dal
{
    public class MasterDbContext : DbContext
    {
        public MasterDbContext(DbContextOptions options) : base(options)
        {
        }
        //public MasterDbContext()
        //{
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        //    .AddJsonFile("appsettings.json")
        //    .Build();

        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"));
        //    }
        //}

        public DbSet<M_Department> Department { get; set; }
        public DbSet<M_Company> Companies { get; set; }
        public DbSet<M_User> User { get; set; }
        public DbSet<M_Menu> NavBarMenu { get; set; }
        public DbSet<M_ArrivalType> ArrivalType { get; set; }
        public DbSet<M_Location> Location { get; set; }
        public DbSet<M_Machine> Machine { get; set; }
        public DbSet<M_Material> Material { get; set; }
        public DbSet<M_MaterialType> MaterialType { get; set; }
        public DbSet<M_Process> Process { get; set; }
        public DbSet<M_Product> Product { get; set; }
        public DbSet<M_Product_Process> ProductProcess { get; set; }
        public DbSet<M_ProductionType> ProductionType { get; set; }
        public DbSet<M_RawMaterialType> RawMaterialType { get; set; }
        public DbSet<M_Unit> Unit { get; set; }
        public DbSet<M_Warehouse> Warehouse { get; set; }
        public DbSet<M_Customer> Customer { get; set; }
        public DbSet<M_Vendor> Vendor { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<M_Company>()
                .HasIndex(p => new { p.CompanyCode })
                .IsUnique(true);

            //Unit more specifications.
            modelBuilder.Entity<M_Unit>()
                .HasIndex(u => new { u.UnitCode })
                .IsUnique(true);

            //Department more specifications.
            modelBuilder.Entity<M_Department>()
                .HasIndex(p => new { p.DeptCode, p.CompanyCode })
                .HasName("IX_DEPTANDCOMP")
                .IsUnique(true);

            //User more specifications.
            modelBuilder.Entity<M_User>()
                .HasIndex(u => new { u.UserCode, u.CompanyCode })
                .HasName("IX_USERANDCOMP")
                .IsUnique(true);

            modelBuilder.Query<M_UserObj>();
            modelBuilder.Query<M_MenuObj>();

            //Arrival Type more specifications.
            modelBuilder.Entity<M_ArrivalType>()
                .HasIndex(at => new { at.ArrivalTypeCode, at.CompanyCode })
                .HasName("IX_ARRTYPEANDCOMP")
                .IsUnique(true);

            modelBuilder.Query<M_LocationObj>();

            //Machine more specifications.
            modelBuilder.Entity<M_Machine>()
                .HasIndex(mc => new { mc.MachineCode, mc.CompanyCode })
                .HasName("IX_MCANDCOMP")
                .IsUnique(true);

            modelBuilder.Query<M_MachineObj>();

            //Material more specifications.
            modelBuilder.Entity<M_Material>()
                .HasIndex(m => new { m.MaterialCode, m.CompanyCode })
                .HasName("IX_MATANDCOMP")
                .IsUnique(true);

            modelBuilder.Query<M_MaterialObj>();

            //Material Type more specifications.
            modelBuilder.Entity<M_MaterialType>()
                .HasIndex(m => new { m.MatTypeCode, m.CompanyCode })
                .HasName("IX_MATTYPECOMP")
                .IsUnique(true);

            //Process more specifications.
            modelBuilder.Entity<M_Process>()
                .HasIndex(p => new { p.ProcessCode, p.CompanyCode })
                .HasName("IX_PROCESSCOMP")
                .IsUnique(true);

            //Product more specifications.
            modelBuilder.Entity<M_Product>()
                .HasIndex(p => new { p.ProductCode, p.CompanyCode })
                .HasName("IX_PRODUCTANDCOMP")
                .IsUnique(true);

            modelBuilder.Query<M_ProductObj>();

            modelBuilder.Query<M_Product_ProcessObj>();

            //Production Type more specifications.
            modelBuilder.Entity<M_ProductionType>()
                .HasIndex(pt => new { pt.ProdTypeCode, pt.CompanyCode })
                .HasName("IX_PRODTYPECOMP")
                .IsUnique(true);

            //Raw Material Type more specifications.
            modelBuilder.Entity<M_RawMaterialType>()
                .HasIndex(rt => new { rt.RawMatTypeCode, rt.CompanyCode })
                .HasName("IX_RAWMATTYPECOMP")
                .IsUnique(true);

            //Warehouse more specifications.
            modelBuilder.Entity<M_Warehouse>()
                .HasIndex(w => new { w.WarehouseCode, w.CompanyCode })
                .HasName("IX_WHANDCOMP")
                .IsUnique(true);

            //Customer more specifications.
            modelBuilder.Entity<M_Customer>()
                .HasIndex(c => new { c.CustomerCode, c.CompanyCode })
                .HasName("IX_CUSTANDCOMP")
                .IsUnique(true);

            //Vendor more specifications.
            modelBuilder.Entity<M_Vendor>()
                .HasIndex(v => new { v.VendorCode, v.CompanyCode })
                .HasName("IX_CUSTANDCOMP")
                .IsUnique(true);
        }
    }
}
