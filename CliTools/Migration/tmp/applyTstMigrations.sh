export PGPASSWORD=bitsoft-group123!
echo Server 	- $1 
echo Port 	- $2 
echo DB 	- $3 
echo fileName 	- $4 

export username=postgres
export server=$1
export port=$2
export database=$3
export dbName=$3

#SET PGCLIENTENCODING=utf-8

psql -h $server -p $port  -U $username -d $database -e -v v1='$dbName' -L migrate_$3.log  -v v2="""$dbName""" -f ../../src/App.Data/Migrations/SQL/$4