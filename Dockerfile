FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app


COPY . .
CMD git submodule update --init --recursive
ENV TZ=Europe/Kiev
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone 

RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs
RUN npm install --global gulp
WORKDIR /app/submodule/css-template

RUN npm i 
RUN gulp build-diklz
RUN gulp transfer

WORKDIR /app/src/App.WebHost
# Restore all packages
RUN dotnet restore App.Host.csproj

# Build the source code
RUN dotnet build App.Host.csproj

RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime

WORKDIR /app

COPY --from=build-env /app/src/App.WebHost/out .
COPY --from=build-env /app/submodule/App.Core/src/App.Core.Utils/PdfConverter/libs/x64/ .

ENV PATH=/app;$PATH
RUN apt-get update
RUN apt-get install wget fontconfig libfreetype6 libx11-6 libxcb1 libxext6 libxrender1 xfonts-75dpi xfonts-base libjpeg62-turbo -y

#RUN wget http://security.debian.org/debian-security/pool/updates/main/o/openssl/libssl1.0.0_1.0.1t-1+deb8u11_amd64.deb
#RUN dpkg -i libssl1.0.0_1.0.1t-1+deb8u11_amd64.deb
RUN wget http://security.debian.org/debian-security/pool/updates/main/o/openssl/libssl1.0.0_1.0.1t-1+deb8u12_amd64.deb
RUN dpkg -i libssl1.0.0_1.0.1t-1+deb8u12_amd64.deb

RUN wget http://ftp.us.debian.org/debian/pool/main/libp/libpng/libpng12-0_1.2.50-2+deb8u3_amd64.deb
RUN dpkg -i libpng12-0_1.2.50-2+deb8u3_amd64.deb

RUN wget https://github.com/wkhtmltopdf/wkhtmltopdf/releases/download/0.12.5/wkhtmltox_0.12.5-1.jessie_amd64.deb
RUN dpkg -i wkhtmltox_0.12.5-1.jessie_amd64.deb

RUN cp /usr/local/bin/wkhtmlto* /usr/bin/
# Обновляем сертификаты
RUN curl -o wwwroot/custom_js/clearJs/Data/CACertificates.p7b https://ca.informjust.ua/download/Soft/CACertificates.p7b
RUN curl -o wwwroot/custom_js/clearJs/Data/CAs.json  https://ca.informjust.ua/download/Soft/CAs.json

ENV ASPNETCORE_ENVIRONMENT Dev
EXPOSE 5060
ENTRYPOINT ["dotnet", "App.Host.dll",  "--urls", "http://0.0.0.0:5060"]