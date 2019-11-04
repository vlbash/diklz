-- Making sure the database exists
SELECT * from pg_database where datname = 'tst_tst';
-- Disallow new connections
UPDATE pg_database SET datallowconn = 'false' WHERE datname = 'tst_soc';
ALTER DATABASE tst_soc CONNECTION LIMIT 1;
-- Terminate existing connections
SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = 'tst_soc';
-- Drop database
DROP DATABASE tst_soc;

