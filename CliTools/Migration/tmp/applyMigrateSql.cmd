@ECHO OFF
SET PGPASSWORD=bitsoft-group123!
::cd ..\..\src\Social.Services.Data
echo Server 	- %1 
echo Port 	- %2 
echo DB 	- %3 
echo fileName 	- %4 

SET username=postgres
SET server=%1
SET port=%2
SET database=%3
SET dbName=%3

SET PGCLIENTENCODING=utf-8
chcp 65001

psql.exe -h %server% -p %port%  -U %username% -d %database% -e -v v1='%dbName%' -L migrate_%3.log -v v2="""%dbName%""" -f ..\..\src\App.Data\Migrations\SQL\%4 