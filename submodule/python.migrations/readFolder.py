#!/bin/python3
import os
import fnmatch
diklzList = []
<<<<<<< HEAD
=======
#for file in os.listdir('../../src/App.Data/Migrations/SQL'):
#    if fnmatch.fnmatch(file,'*.sql'):
#        diklzList.append(file)
#diklzList.sort()
#print(diklzList)
#for list in diklzList:
#    print(list)    
>>>>>>> daa7eb4f53b83c345ba6c9934dd34b7918faa8eb
for file in os.listdir('../../src/App.Data/Migrations/Procedures'):
    if fnmatch.fnmatch(file,'*.sql'):
        file='../../src/App.Data/Migrations/Procedures/'+file    
        fd=open(file, 'r', encoding='utf-8-sig')
        print(fd.read())
       # diklzList.append(file)
diklzList.sort()
print(diklzList)
#for list in diklzList:
 #   print(list)    