--Copy the following command and run in psql tool or psql commandline.
\echo SCRIPT RUN STARTS;

\set ON_ERROR_STOP on
--Setting default_transaction_read_only globally to ON forces all connections to disallow writes to the database.
--Ref. https://www.postgresql.org/docs/current/runtime-config-client.html 
SET default_transaction_read_only = off; 

--Change to client_encoding to UTF8.
SET client_encoding = 'UTF8';

--This controls whether ordinary string literals ('...') treat backslashes literally, as specified in the SQL standard. 
--Ref. https://www.postgresql.org/docs/current/runtime-config-compatible.html
--SET standard_conforming_strings = on;

--
-- Roles
--
SELECT 'CREATE ROLE user_owner WITH NOSUPERUSER INHERIT NOCREATEROLE NOCREATEDB LOGIN NOREPLICATION NOBYPASSRLS PASSWORD ''user_owner''' WHERE NOT EXISTS (select * from pg_roles WHERE rolname = 'user_owner')\gexec
SELECT 'CREATE ROLE user_viewer WITH NOSUPERUSER INHERIT NOCREATEROLE NOCREATEDB LOGIN NOREPLICATION NOBYPASSRLS PASSWORD ''user_viewer''' WHERE NOT EXISTS (select * from pg_roles WHERE rolname = 'user_viewer')\gexec
SELECT 'CREATE ROLE user_writer WITH NOSUPERUSER INHERIT NOCREATEROLE NOCREATEDB LOGIN NOREPLICATION NOBYPASSRLS PASSWORD ''user_writer''' WHERE NOT EXISTS (select * from pg_roles WHERE rolname = 'user_writer')\gexec

\echo 'ROLE SCRIPT ENDS'

\connect template1

--
-- Name: user; Type: DATABASE; Schema: -; Owner: user_owner
--

SELECT 'CREATE DATABASE "user"  WITH ENCODING = UTF8 LOCALE = ''en_US.UTF-8'' TEMPLATE = template0' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'user')\gexec

ALTER DATABASE "user" OWNER TO user_owner;

\connect "user"

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false); -- default path is user$, public. Make it to blank by setting it to false. Then you follow dbname\schemaname\table
SET check_function_bodies = false; -- https://www.postgresql.org/docs/current/runtime-config-client.html
SET xmloption = content;
SET client_min_messages = notice;
SET row_security = off;

CREATE SCHEMA IF NOT EXISTS login AUTHORIZATION postgres;

ALTER SCHEMA login OWNER TO user_owner;

--ALTER TABLE IF EXISTS login."user" DROP CONSTRAINT IF EXISTS "userId_fk";


SET default_tablespace = '';
SET default_table_access_method = heap;

 -- below script to create the table 

 DO
$do$
BEGIN
    CREATE TABLE IF NOT EXISTS login."versionControl"
    (
        id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
        "majorVersion" text COLLATE pg_catalog."default" NOT NULL,
        "minorVersion" text COLLATE pg_catalog."default" NOT NULL,
        "patch" text COLLATE pg_catalog."default" NOT NULL,
        "build" text COLLATE pg_catalog."default" NOT NULL,
        "createdDate" timestamp with time zone,
        "dateModified" timestamp with time zone,
        "userCreated" text DEFAULT CURRENT_USER,
        "userModified" text COLLATE pg_catalog."default" NOT NULL,
        CONSTRAINT "loginDbversion_pkey" PRIMARY KEY (id)
    );
    COMMENT ON TABLE login."versionControl"
        IS 'Created by Dev for storing the db version.';
    ALTER TABLE IF EXISTS login."versionControl" OWNER TO user_owner;
END
$do$;

DO
$$
BEGIN
    IF EXISTS(select 1 FROM login."versionControl" where "majorVersion"='1' and "minorVersion"='0' and "patch"='0' and "build"='0' LIMIT 1)
        THEN
            RAISE NOTICE 'login 1.0.0.0 version already exists. Skipping.';
    ELSE
        BEGIN
			
        CREATE TABLE IF NOT EXISTS login.user
        (
	        id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
	        username text  NOT NULL,
	        email text  NOT NULL,
	        password text  NOT NULL,       
	        "roleId" integer  NOT NULL,       
	        "dateCreated" timestamp without time zone DEFAULT (timezone('utc', now())),
	        "dateModified" timestamp without time zone DEFAULT (timezone('utc', now())),
	         CONSTRAINT "user_pkey" PRIMARY KEY (id)
         );
 
        ALTER TABLE IF EXISTS login.user OWNER TO user_owner;

        CREATE TABLE IF NOT EXISTS login.role
        (
	        id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
	        code text  NOT NULL,
	        description text  NOT NULL,      
	        "dateCreated" timestamp without time zone DEFAULT (timezone('utc', now())),
	        "dateModified" timestamp without time zone DEFAULT (timezone('utc', now())),
	         CONSTRAINT "roles_pkey" PRIMARY KEY (id)
         );
 
        ALTER TABLE IF EXISTS login.role OWNER TO user_owner;
			 
         --INSERT SCRIPTS           
          raise notice 'INSERT CHECK or REAL INSERT ON all TABLE';  
        --\echo 'INSERT CHECK or REAL INSERT ON all TABLE'

--roles

    IF EXISTS ( SELECT FROM pg_tables WHERE schemaname = 'login' AND tablename = 'roles' )
        THEN
        TRUNCATE TABLE ONLY login."user" RESTART IDENTITY;
 
         INSERT INTO login.roles(code) VALUES ('admin'),('user');
                 
        ELSE
            raise notice 'Table login."operationStatus" does not exist';
    END IF;



-- -- re assigning foreign keys 
             
        IF NOT EXISTS (select constraint_name from information_schema.table_constraints where table_schema = 'login' and table_name = 'user' and constraint_name = 'roleId_fk' and constraint_type = 'FOREIGN KEY')   
        THEN   
            ALTER TABLE ONLY login.user  
            ADD CONSTRAINT "roleId_fk" FOREIGN KEY ("roleId") REFERENCES login.role(id) ON UPDATE CASCADE;   
        ELSE   
            raise notice 'Foreign Key "roleId_fk" on user already exist';   
        END IF;

               -- insert version 
            INSERT INTO login."versionControl"("majorVersion", "minorVersion", "patch","build","createdDate","dateModified","userModified") VALUES ('1', '0','0','0',timeofday()::TIMESTAMP,timeofday()::TIMESTAMP,'');

     END;
    END IF;
    COMMIT;
END;
$$;

GRANT TEMPORARY ON DATABASE "user" TO PUBLIC;
GRANT CONNECT ON DATABASE "user" TO user_owner;
GRANT CONNECT ON DATABASE "user" TO user_viewer;
GRANT CONNECT ON DATABASE "user" TO user_writer;

GRANT USAGE ON SCHEMA login TO user_viewer;
GRANT ALL ON SCHEMA login TO user_owner;
GRANT ALL ON SCHEMA login TO user_writer;

GRANT ALL ON TABLE login.user TO user_owner;
GRANT ALL ON TABLE login.role TO user_owner;

GRANT SELECT ON TABLE login."versionControl" TO user_viewer;
GRANT SELECT ON TABLE login.user TO user_viewer;
GRANT SELECT ON TABLE login.role TO user_viewer;

GRANT SELECT,INSERT,UPDATE ON TABLE login."versionControl" TO user_writer;
GRANT SELECT,INSERT,UPDATE ON TABLE login.user TO user_writer;
GRANT SELECT,INSERT,UPDATE ON TABLE login.role TO user_writer;

REVOKE CONNECT,TEMPORARY ON DATABASE "user" FROM PUBLIC;

\echo 'SCRIPT COMPLETE FOR user DB'
