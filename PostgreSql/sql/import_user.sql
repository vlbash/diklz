CREATE DATABASE :v1
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Ukrainian_Ukraine.1251'
    LC_CTYPE = 'Ukrainian_Ukraine.1251'
    CONNECTION LIMIT = -1;
CREATE USER :v2 WITH password :v3;
GRANT ALL ON DATABASE :v1 TO :v2;