@ECHO OFF
call tmp\applyMigrateSql.cmd 52.136.239.225 58432 dev_rehab Init\fill-DtmData-1.sql
call tmp\applyMigrateSql.cmd 52.136.239.225 58432 dev_rehab Init\fill-PEDI-DtmData-2.sql

pause