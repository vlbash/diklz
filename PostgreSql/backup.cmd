::pg_dump --host '52.136.239.225' --port 58432 --username "postgres" --no-password --format custom --blobs --file "C:/backups/DB/tst_soc.dmp" "tst_soc"
::pg_dump --host 52.136.239.225 --port 58432 --username "postgres" --format custom --blobs --file "C:/backups/DB/tst_soc.dmp" "dev_soc"

::pg_dump -h 52.136.239.225 -p 58432 -U socserv database > tmp.sql
::psql.exe -h %server% -p %port%  -U %username% -d %database% -e -v v1='%dbName%' -v v2="""%dbName%""" -f sql\dropDB.sql

::pg_dump -h 52.136.239.225 -p 58432  -U socserv -d %database% --format custom --blobs --file "C:/backups/DB/dev_soc.dmp" "dev_soc"
::pg_dump -h 52.136.239.225 -p 58432 -U socserv -W -F c dev_soc > C:/backups/DB/dev_soc.dmp

pg_restore.exe --host "52.136.239.225" --port "58432" --username "socserv" --dbname "tst_soc" --verbose "C:\\backups\\DB\\tst_soc"
--file "C:\\backups\\DB\\soc_tst.dmp" --host "10.14.22.2" --port "5432" --username "postgres" --no-password --verbose --format=c --blobs "tst_soc"