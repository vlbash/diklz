#!/usr/bin/python
import os
import fnmatch
import psycopg2
from config import config
import logging

LOG_FILENAME = 'migrations.log'
format_string = '%(levelname)s: %(asctime)s: %(message)s'
logging.basicConfig(level=logging.DEBUG, filename='migrations.log', format=format_string)

def procedures():
    """ run migrations in the PostgreSQL database"""
    logging.info('Run migration for next scripts \n' + s)
    conn = None
    try:
        # read the connection parameters
        params = config()
        # connect to the PostgreSQL server
        conn = psycopg2.connect(**params)
        cur = conn.cursor()
        # create table one by one
        for file in os.listdir('../../src/App.Data/Migrations/Procedures'):
            if fnmatch.fnmatch(file,'*.sql'):
                fd=open(file, 'r', encoding='utf-8-sig')
                print(file)
                cur.execute(fd.read())
                logging.info('Script '+file+' is success')
                cur.execute(fd.read())
                logging.info('Script '+file+' is success')
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
    procedures()