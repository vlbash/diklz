@ECHO OFF
set DEPLOY_LOCATION=Local
cd ..\..\src\App.Data
::set version=%1 
::echo %version%
::dotnet ef migrations add %1%  -c ApplicationDbContext
dotnet ef migrations add InitialCreate -c MigrationDbContext
