-- Making sure the database exists
SELECT * from pg_database where datname = :v1;

-- Disallow new connections
UPDATE pg_database SET datallowconn = 'false' WHERE datname = :v1;
ALTER DATABASE :v1 CONNECTION LIMIT 1;

-- Terminate existing connections
SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = :v1;

-- Drop database
DROP DATABASE :v2;
