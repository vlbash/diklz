SET PGPASSWORD=socServ
::@ECHO OFF
::cd ..\..\src\Social.Services.Data
echo remove DB - %1 
::dotnet ef migrations add %1%  -c ApplicationDbContext

:: Lfqrjv-[f,123!
::psql.exe -h 52.136.239.225 -p 58432 -U socserv -d postgres -f dropUser-force.sql 

::echo "Type DB name for drop"
SET server=52.136.239.225
::SET /P server="Server [%server%]: "

SET database=postgres
::SET /P database="Database [%database%]: "

SET port=58432
::SET /P port="Port [%port%]: "

SET username=socserv
::SET /P username="Username [%username%]: "

SET dbName=%1
::SET /P dbName="Enter DB name for drop [%dbName%]: "
::ECHO you typed %dbName%
::SET PGPASSWORD="socServ";  TODO : !?

psql.exe -h %server% -p %port%  -U %username% -d %database% -e -v v1='%dbName%' -v v2="""%dbName%""" -f sql\dropDB.sql

pause