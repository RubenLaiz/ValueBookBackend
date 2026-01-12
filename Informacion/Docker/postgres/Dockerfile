# Usa la última imagen de PostgreSQL
FROM postgres:latest

# Establece las variables de entorno necesarias para configurar PostgreSQL
ENV POSTGRES_USER=admin
ENV POSTGRES_PASSWORD=admin123
ENV POSTGRES_DB=valuebook

# Copia el archivo de inicialización SQL al directorio de inicialización de Docker
COPY init-db.sql /docker-entrypoint-initdb.d/

# Asegúrate de que el archivo tenga permisos de ejecución
RUN chmod 755 /docker-entrypoint-initdb.d/init-db.sql
