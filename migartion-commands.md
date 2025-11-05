# Sqlite
dotnet ef migrations add {migration-name} -c MyDbContext -p SqliteMigrations/SqliteMigrations.csproj -s SqliteMigrations/SqliteMigrations.csproj -o Migrations
dotnet ef database update -p SqliteMigrations/SqliteMigrations.csproj -s SqliteMigrations/SqliteMigrations.csproj

# SqlServer
dotnet ef migrations add {migration-name} -c MyDbContext -p SqlServerMigrations/SqlServerMigrations.csproj -s SqlServerMigrations/SqlServerMigrations.csproj -o Migrations
dotnet ef database update -p SqlServerMigrations/SqlServerMigrations.csproj -s SqlServerMigrations/SqlServerMigrations.csproj