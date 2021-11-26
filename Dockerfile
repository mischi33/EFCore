FROM mcr.microsoft.com/mssql/server:2017-latest-ubuntu

ENV SA_PASSWORD Test123!
ENV ACCEPT_EULA Y
ENV MSSQL_PID Express


RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app
COPY . /usr/src/app

RUN chmod +x /usr/src/app/run-initialization.sh

CMD /bin/bash ./entrypoint.sh

# docker build -t db-snackmachine .
# docker run -p 1433:1433 -d db-snackmachine