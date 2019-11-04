@echo off
set h=%TIME:~0,2%
set m=%TIME:~3,2%
set s=%TIME:~6,2%
set ms=%TIME:~9,2%
set curtime=%h%:%m%:%s%:%ms%
SET server=52.136.239.225
SET port=58432
SET /P dbName="Database [%dbName%]: " 
SET /P uName="UserName [%uName%]: " 
echo " %curtime%  |  Server  [%server%] Database [%dbName%] Backup_file [%bk_file%]" >> drop_import.log
echo " %curtime%  |  Username [%uName%] PassWord [%pass%] " >>import.log
echo ------------------------------------------IF YOU are SURE, click enter-----------------------------------------------
pause
set PGPASSWORD=Lfqrjv-[f,123!&&pg_dump\psql.exe -h %server% -p %port% -U postgres -d postgres -e -v v1=%dbName% -v v2=%uName% -f sql\import_drop.sql>drop_import.log
