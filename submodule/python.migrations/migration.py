#!/usr/bin/python
import os
import fnmatch
import psycopg2
from config import config
import logging

LOG_FILENAME = 'migrations.log'
format_string = '%(levelname)s: %(asctime)s: %(message)s'
logging.basicConfig(level=logging.DEBUG, filename='migrations.log', format=format_string)

def migrations():
    path='../../src/App.Data/Migrations/SQL/'
    list_sql=[]
    """ run migrations in the PostgreSQL database"""
    fileMigrations = path + 'diklzList'
    ls=open(fileMigrations, 'r', encoding='utf-8-sig')
    s=ls.read()
    list_sql=s.split('\n')
    print(list_sql)
    logging.info('Run migration for next scripts \n' + s)
  
    conn = None
    try:
        # read the connection parameters
        params = config()
        # connect to the PostgreSQL server
        conn = psycopg2.connect(**params)
        cur = conn.cursor()
        # create table one by one
        for sql in list_sql:
            sql = path + sql
            fd=open(sql, 'r', encoding='utf-8-sig')
            print(sql)
            cur.execute(fd.read())
            logging.info('Script '+sql+' is success')
        # close communication with the PostgreSQL database server
        cur.close()
        # commit the changes
        conn.commit()
    except (Exception, psycopg2.DatabaseError) as error:
        print(error)
    finally:
        if conn is not None:
            conn.close()
 
if __name__ == '__main__':
    migrations()