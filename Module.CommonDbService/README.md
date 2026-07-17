```

MSSql
dotnet ef dbcontext scaffold "Server=.;Database=WSIMS-ERP;User Id=sa;Password=sasa@123;TrustServerCertificate=True;MultipleActiveResultSets=True;Connection Timeout=30" Microsoft.EntityFrameworkCore.SqlServer -o EfAppDbContextModels -c AppDbContext -f

Postgre
dotnet ef dbcontext scaffold "Host=localhost;Database=WSIMS-ERP;Username=postgres;Password=sasa@123;Port=5432;" Npgsql.EntityFrameworkCore.PostgreSQL -o EfAppDbContextModels -c AppDbContext -f

Postgre Neno Server
dotnet ef dbcontext scaffold "Host=ep-bold-bonus-azbxwgr9.c-3.ap-southeast-1.aws.neon.tech;Database=WSIMS-ERP;Username=neondb_owner;Password=npg_94yTctfwHjEB;Port=5432;SSL Mode=Require;" Npgsql.EntityFrameworkCore.PostgreSQL -o EfAppDbContextModels -c AppDbContext -f

```