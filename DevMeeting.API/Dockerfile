FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgdiplus libc6-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build

RUN mkdir -p /usr/share/man/man1 /usr/share/man/man2

RUN apt-get update && \
apt-get install -y --no-install-recommends \
        openjdk-11-jre

COPY . ./
# restauração dos pacotes
RUN dotnet restore DevMeeting.API/DevMeeting.API.csproj 
RUN dotnet restore DevMeeting.API.Tests/DevMeeting.API.Tests.csproj 

# rodando testes coletanto o code coverage 
RUN dotnet test DevMeeting.API.Tests/DevMeeting.API.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# instalação do SonarQube dentro do container, utilizando a versão client do SonarQube junto ao dotnet ao invés de realizar o download do zip e, utilizar os executáveis de forma manual.
RUN dotnet tool install -g dotnet-sonarscanner

# adiciona o caminho do executável do scanner do sonar na váriavel de ambiente do sistema
ENV PATH="${PATH}:/root/.dotnet/tools"

RUN dotnet sonarscanner begin /k:"DevMeeting" /d:sonar.host.url="http://172.17.0.2:9000" /d:sonar.verbose=true  /v:1.0.0 /d:sonar.login="422247bd3013a0944fe8742169b5816959957082" /d:sonar.cs.opencover.reportsPaths="./DevMeeting.API.Tests/coverage.opencover.xml" /d:sonar.coverage.exclusions="**Tests*.cs"
RUN dotnet build
RUN dotnet sonarscanner end /d:sonar.login="422247bd3013a0944fe8742169b5816959957082"
