#!/usr/bin/sh
#expect -f
spawn ssh -p 2210 vbash@62.205.144.40
expect -exact "Are you sure you want to continue connecting (yes/no)? "
send -- "yes\r"
expect -exact "vbash@62.205.144.40's password: "
send -- "vbash\r"
interact
