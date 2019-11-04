@ECHO OFF
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab Init\fill-DtmData-1.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab Init\fill-PEDI-DtmData-2.sql

pause