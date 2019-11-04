@ECHO OFF
set DEPLOY_LOCATION=Local
cd ..\..\src\App.Data
echo add migration - %1 
::set version=%1 
::echo %version%
dotnet ef migrations add %1%  -c MigrationDbContext
pause