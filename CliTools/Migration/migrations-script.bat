@ECHO OFF
set DEPLOY_LOCATION=Local
cd ..\..\src\App.Data
echo generate script for migration - %1 %2
dotnet ef migrations script %1 %2 -i -o ..\..\src\App.Data\Migrations\SQL\%2.sql -c MigrationDbContext 
echo %2%.sql >> Migrations\SQL\diklzList