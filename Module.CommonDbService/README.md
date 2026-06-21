```

dotnet ef dbcontext scaffold "Server=.;Database=WSIMS-ERP;User Id=sa;Password=sasa@123;TrustServerCertificate=True;MultipleActiveResultSets=True;Connection Timeout=30" Microsoft.EntityFrameworkCore.SqlServer -o EfAppDbContextModels -c AppDbContext -f

```