RD "C:\Publish\KyivId" /S /Q
call dotnet publish ..\..\src\App.Cabinet.Host\App.Cabinet.Host.csproj -f netcoreapp2.0 -o C:\Publish\KyivId --runtime win10-x64 -c release
                          
