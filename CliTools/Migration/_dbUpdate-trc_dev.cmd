@ECHO OFF
SET dbServer=%1
SET port=%2
SET database=%3

call tmp\applyMigrateSql.cmd %dbServer% %port% %database% v2.0.sql
call tmp\applyMigrateSql.cmd %dbServer% %port% %database% v2.1.sql
...
