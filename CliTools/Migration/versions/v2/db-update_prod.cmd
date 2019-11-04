@ECHO OFF
:: DDL
::call tmp\applyMigrateSql.cmd 10.14.22.2 5432 soc v1.0.sql
::call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc v1.1.sql /*example

call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc v2.0.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc v2.1.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc v2.2.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc v2.2.1_dml_soc.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc v2.3.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc v2.2.2_dml_soc.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc v2.4.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc v2.5.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc v2.6.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc v2.7.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc v2.8.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc v2.9.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc v2.10.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc v2.11.sql

::view tables
call call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc Views\ClientCardPOCO.sql
call call tmp\applyProdMigrations.cmd 10.14.22.2 5432 soc Views\CitizenCardPOCO.sql

call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab v2.0.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab v2.1.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab v2.2.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab v2.2.1_dml_rehab.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab v2.3.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab v2.2.2_dml_soc.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab v2.4.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab v2.5.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab v2.6.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab v2.7.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab v2.8.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab v2.9.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab v2.10.sql
call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab v2.11.sql

::view tables
call call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab Views\ClientCardPOCO.sql
call call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab Views\CitizenCardPOCO.sql

pause


:: DML
::call tmp\applyMigrateSql.cmd 10.14.22.2 5432 soc v1.3_dml.sql
::call tmp\applyMigrateSql.cmd 10.14.22.2 5432 soc v1.3.sql
::call tmp\applyMigrateSql.cmd 10.14.22.2 5432 soc v1.4.sql


::call tmp\applyProdMigrations.cmd 10.14.22.2 5432 tst_rehab v1.1.sql /*example



:: DML
::call tmp\applyMigrateSql.cmd 10.14.22.2 5432 tst_rehab v1.3_dml.sql

