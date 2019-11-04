RD "C:\Publish\cost" /S /Q
call dotnet publish ..\..\src\App.WebHost\App.WebHost.csproj -f netcoreapp2.1 -o C:\Publish\cost\win10 --runtime win10-x64 -c Debug
