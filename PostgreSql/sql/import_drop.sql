SELECT pg_terminate_backend(pg_stat_activity.pid) 
FROM pg_stat_activity 
WHERE datname=':v1' AND pid<>pg_backend_pid();

drop database :v1;

drop user :v2;