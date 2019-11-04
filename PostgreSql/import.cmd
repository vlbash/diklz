@echo off
::set dbName=%1
set h=%TIME:~0,2%
set m=%TIME:~3,2%
set s=%TIME:~6,2%
set ms=%TIME:~9,2%
set curtime=%h%:%m%:%s%:%ms%
::SET server=192.168.25.11
SET server=52.136.239.225
SET port=58432
SET /P dbName="Database [%dbName%]: " 
SET /P uName="UserName [%uName%]: " 
SET /P passWord="PassWord [%passWord%]: " 
set pass='%passWord%'
::SET /P bk_file="Backup_file [%bk_file%]: "
SET bk_file=C:\backups\DB\tst_rehab.backup
echo " %curtime%  |  Server  [%server%] Database [%dbName%] Backup_file [%bk_file%]" > connect.txt
echo " %curtime%  |  Username [%uName%] PassWord [%pass%] " >>connect.txt
pause
set PGPASSWORD=Lfqrjv-[f,123!&&pg_dump\psql.exe -h %server% -p %port% -U postgres -tAc "SELECT 1 FROM pg_database WHERE datname='%dbName%'" > flag.txt
set /P flag=<flag.txt
echo %flag%
pause
IF /I "%flag%" EQU "1" (
     echo " %curtime%  |  Database [%dbName%] already exist. Use importDROP.cmd ">import.log
) ELSE (
     set PGPASSWORD=Lfqrjv-[f,123!&&pg_dump\psql.exe -h %server% -p %port% -U postgres -d postgres -e -v v1=%dbName% -v v2=%uName% -v v3=%pass% -f sql\import_user.sql>>import.log
     set PGPASSWORD=%passWord%&&pg_dump\pg_restore.exe -h %server% -p %port% -U %uName% -d %dbName% -v %bk_file%>>import.log 
)
del flag.txt
