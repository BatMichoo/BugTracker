FROM mcr.microsoft.com/mssql/server:2022-CU20-ubuntu-22.04
USER root

RUN apt-get update

RUN apt-get install -y --fix-missing gnupg curl

RUN curl -o microsoft.asc https://packages.microsoft.com/keys/microsoft.asc

RUN apt-key add microsoft.asc

RUN rm microsoft.asc

RUN curl https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/prod.list > /etc/apt/sources.list.d/mssql-release.list

RUN apt-get update

RUN ACCEPT_EULA=Y apt-get install -y mssql-tools18 unixodbc-dev

USER mssql
