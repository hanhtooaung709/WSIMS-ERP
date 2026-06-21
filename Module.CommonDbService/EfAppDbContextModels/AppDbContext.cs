using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblBox> TblBoxes { get; set; }

    public virtual DbSet<TblBranch> TblBranches { get; set; }

    public virtual DbSet<TblCurrency> TblCurrencies { get; set; }

    public virtual DbSet<TblDistrict> TblDistricts { get; set; }

    public virtual DbSet<TblPackage> TblPackages { get; set; }

    public virtual DbSet<TblProduct> TblProducts { get; set; }

    public virtual DbSet<TblReqPackage> TblReqPackages { get; set; }

    public virtual DbSet<TblReqPackageChange> TblReqPackageChanges { get; set; }

    public virtual DbSet<TblReqProduct> TblReqProducts { get; set; }

    public virtual DbSet<TblReqProductChange> TblReqProductChanges { get; set; }

    public virtual DbSet<TblReqStockTran> TblReqStockTrans { get; set; }

    public virtual DbSet<TblReqWarehouseUser> TblReqWarehouseUsers { get; set; }

    public virtual DbSet<TblReqWarehouseUserChange> TblReqWarehouseUserChanges { get; set; }

    public virtual DbSet<TblState> TblStates { get; set; }

    public virtual DbSet<TblTownship> TblTownships { get; set; }

    public virtual DbSet<TblWarehouseRole> TblWarehouseRoles { get; set; }

    public virtual DbSet<TblWarehouseUser> TblWarehouseUsers { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=WSIMS-ERP;User Id=sa;Password=sasa@123;TrustServerCertificate=True;MultipleActiveResultSets=True;Connection Timeout=30");
*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblBox>(entity =>
        {
            entity.HasKey(e => e.BoxId).HasName("PK_Tbl_BoxSize");

            entity.ToTable("Tbl_Box");

            entity.Property(e => e.BoxId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BoxCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDateTime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Size)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblBranch>(entity =>
        {
            entity.HasKey(e => e.BranchId);

            entity.ToTable("Tbl_Branch");

            entity.Property(e => e.BranchId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.BranchCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TownshipCode)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblCurrency>(entity =>
        {
            entity.HasKey(e => e.CurrencyId);

            entity.ToTable("Tbl_Currency");

            entity.Property(e => e.CurrencyId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CurrencyDescription).HasMaxLength(50);
            entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblDistrict>(entity =>
        {
            entity.HasKey(e => e.DistrictId);

            entity.ToTable("Tbl_District");

            entity.Property(e => e.DistrictId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DistrictCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DistrictName).HasMaxLength(100);
            entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StateCode)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId);

            entity.ToTable("Tbl_Package");

            entity.Property(e => e.PackageId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BoxCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.BranchCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PackageName).HasMaxLength(100);
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SupplierName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId);

            entity.ToTable("Tbl_Product");

            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProductName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblReqPackage>(entity =>
        {
            entity.HasKey(e => e.ReqPackageId);

            entity.ToTable("Tbl_ReqPackage");

            entity.Property(e => e.ReqPackageId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ApprovedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BoxCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.BranchCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PackageId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PackageName).HasMaxLength(100);
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RejectReason)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ReqDateTime).HasColumnType("datetime");
            entity.Property(e => e.ReqUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasMaxLength(10);
            entity.Property(e => e.SupplierName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblReqPackageChange>(entity =>
        {
            entity.HasKey(e => e.ReqPackageChangesId);

            entity.ToTable("Tbl_ReqPackageChanges");

            entity.Property(e => e.ReqPackageChangesId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ApprovedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BoxCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PackageId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PackageName).HasMaxLength(100);
            entity.Property(e => e.ProductsId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RejectReason)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ReqDateTime).HasColumnType("datetime");
            entity.Property(e => e.ReqUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SupplierName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblReqProduct>(entity =>
        {
            entity.HasKey(e => e.ReqProductId);

            entity.ToTable("Tbl_ReqProduct");

            entity.Property(e => e.ReqProductId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ApprovedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.RejectReason)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ReqDateTime).HasColumnType("datetime");
            entity.Property(e => e.ReqUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblReqProductChange>(entity =>
        {
            entity.HasKey(e => e.ReqProductChangesId);

            entity.ToTable("Tbl_ReqProductChanges");

            entity.Property(e => e.ReqProductChangesId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ApprovedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.RejectReason)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ReqDateTime).HasColumnType("datetime");
            entity.Property(e => e.ReqUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblReqStockTran>(entity =>
        {
            entity.HasKey(e => e.ReqStockTranId);

            entity.ToTable("Tbl_ReqStockTran");

            entity.Property(e => e.ReqStockTranId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BranchCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PackageId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RejectReason)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ReqDateTime).HasColumnType("datetime");
            entity.Property(e => e.ReqUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblReqWarehouseUser>(entity =>
        {
            entity.HasKey(e => e.ReqWarehouseUserId);

            entity.ToTable("Tbl_ReqWarehouseUser");

            entity.Property(e => e.ReqWarehouseUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ApprovedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ReqDateTime).HasColumnType("datetime");
            entity.Property(e => e.ReqUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WarehouseUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblReqWarehouseUserChange>(entity =>
        {
            entity.HasKey(e => e.ReqWarehouseUserChangesId);

            entity.ToTable("Tbl_ReqWarehouseUserChanges");

            entity.Property(e => e.ReqWarehouseUserChangesId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ApprovedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BranchCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ChangesType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.LoginPassword)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ReqDateTime).HasColumnType("datetime");
            entity.Property(e => e.ReqUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.WarehouseUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblState>(entity =>
        {
            entity.HasKey(e => e.StateId);

            entity.ToTable("Tbl_State");

            entity.Property(e => e.StateId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedUserId).HasMaxLength(50);
            entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedUserId).HasMaxLength(50);
            entity.Property(e => e.StateCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StateName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblTownship>(entity =>
        {
            entity.HasKey(e => e.TownshipId);

            entity.ToTable("Tbl_Township");

            entity.Property(e => e.TownshipId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DistrictCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TownshipCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TownshipName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblWarehouseRole>(entity =>
        {
            entity.HasKey(e => e.WarehouseRoleId);

            entity.ToTable("Tbl_WarehouseRole");

            entity.Property(e => e.WarehouseRoleId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblWarehouseUser>(entity =>
        {
            entity.HasKey(e => e.WarehouseUserId);

            entity.ToTable("Tbl_WarehouseUser");

            entity.Property(e => e.WarehouseUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BranchCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.LoginPassword)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedUserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.RoleCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
