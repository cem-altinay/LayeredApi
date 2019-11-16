Adding a Migration
    Package Manager Console => add-migration MigrationName
	CLI => dotnet ef migrations add MigrationName

Creating or Updating the Database
	Package Manager Console => Update-Database
	CLI => dotnet ef database update


Remove Migration
	Package Manager Console => remove-migration
	CLI => dotnet ef migrations remove

Reverting a Migration
	Package Manager Console => remove-migration
	CLI => dotnet ef migrations remove

DB First 
Scaffold-DbContext "Server=DESKTOP-TB99DGO; Persist Security Info=True;Database=expenses.main; User=sa;Password=123456;" Microsoft.EntityFrameworkCore.SqlServer  -Context "MainDbContext" -DataAnnotations
	