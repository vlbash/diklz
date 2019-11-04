RD "C:\Publish\soc" /S /Q
call dotnet publish ..\..\src\Social.Services.Host\App.Host.csproj -f netcoreapp2.0 -o C:\Publish\soc\Centos --runtime centos.7-x64
