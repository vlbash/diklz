#!/usr/bin/sh
TIME="10"
cd submodule/python.migrations
pip3 install -r requirements.txt
python3 migration.py