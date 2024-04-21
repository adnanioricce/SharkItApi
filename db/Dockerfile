FROM postgres:14

COPY createdb.sql /docker-entrypoint-initdb.d/

RUN chmod +r /docker-entrypoint-initdb.d/createdb.sql
