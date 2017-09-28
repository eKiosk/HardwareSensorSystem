# HardwareSensorSystem

## Entity Framework Core migrations

Change to the directory .\src\HardwareSensorSystem and execute the following commands
```
$ dotnet ef migrations add InitialCreate --context ApplicationDbContext --output-dir Migrations\ApplicationDb
$ dotnet ef migrations add InitialCreate --context ConfigurationDbContext --output-dir Migrations\ConfigurationDb
$ dotnet ef migrations add InitialCreate --context PersistedGrantDbContext --output-dir Migrations\PersistedGrantDb
```
