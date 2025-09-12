FROM mcr.microsoft.com/mssql/server:2022-CU20-ubuntu-22.04
USER root

# Update package list
RUN apt-get update

# Install gnupg and curl 
RUN apt-get install -y --fix-missing gnupg curl

# Download the Microsoft repository key
RUN curl -o microsoft.asc https://packages.microsoft.com/keys/microsoft.asc

# Add the key
RUN apt-key add microsoft.asc

# Clean up the downloaded key file
RUN rm microsoft.asc

# Add Microsoft repository for mssql-tools
RUN curl https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/prod.list > /etc/apt/sources.list.d/mssql-release.list

# Update package list again
RUN apt-get update

# Install mssql-tools and unixodbc-dev
RUN ACCEPT_EULA=Y apt-get install -y mssql-tools18 unixodbc-dev

# Set the default user back to mssql
USER mssql
