@ECHO OFF
call _dbUpdate-dev_rehab.cmd 10.14.22.2 5432 tst_rehab 
call _dbUpdate-dev_soc.cmd 10.14.22.2 5432 tst_soc 

