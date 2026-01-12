-- Crear la base de datos solo si no existe
DO
$$
BEGIN
   IF NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = 'valuebook') THEN
      PERFORM dblink_exec('dbname=postgres', 'CREATE DATABASE valuebook');
   END IF;
END
$$;
