@ECHO OFF

echo create DB - %1 
SET PGPASSWORD=socServ

::echo "Type DB name for drop"
SET server=52.136.239.225
::SET /P server="Server [%server%]: "

SET database=postgres
SET database1=%1
echo Active DB - %1 
::SET /P database="Database [%database%]: "

SET port=58432
::SET /P port="Port [%port%]: "

SET username=socserv
::SET /P username="Username [%username%]: "

SET dbName=%1
::SET /P dbName="Enter DB name for drop [%dbName%]: "
::ECHO you typed %dbName%
::SET PGPASSWORD="socServ";  TODO : !?

psql.exe -h %server% -p %port%  -U %username% -d %database% -e -v v1=%dbName% -v v2="""%dbName%""" -f sql\createDB.sql
::psql.exe -h %server% -p %port%  -U %username% -d %database1% -e -v v1=%dbName% -v v2="""%dbName%""" -f sql\createMigrationTable.sql
