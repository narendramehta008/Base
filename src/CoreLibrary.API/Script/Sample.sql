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
SELECT 'CREATE ROLE corelib_viewer WITH NOSUPERUSER INHERIT NOCREATEROLE NOCREATEDB LOGIN NOREPLICATION NOBYPASSRLS PASSWORD ''corelib_viewer''' WHERE NOT EXISTS (select * from pg_roles WHERE rolname = 'corelib_viewer')\gexec
SELECT 'CREATE ROLE corelib_writer WITH NOSUPERUSER INHERIT NOCREATEROLE NOCREATEDB LOGIN NOREPLICATION NOBYPASSRLS PASSWORD ''corelib_writer''' WHERE NOT EXISTS (select * from pg_roles WHERE rolname = 'corelib_writer')\gexec
SELECT 'CREATE ROLE corelib_owner WITH NOSUPERUSER INHERIT NOCREATEROLE NOCREATEDB LOGIN NOREPLICATION NOBYPASSRLS PASSWORD ''corelib_owner''' WHERE NOT EXISTS (select * from pg_roles WHERE rolname = 'corelib_owner')\gexec

\echo 'ROLE SCRIPT ENDS'


--
-- Name: user; Type: DATABASE; Schema: -; Owner: corelib_owner
--

SELECT 'CREATE DATABASE "coreLib"  WITH ENCODING = UTF8 LOCALE = ''en_US.UTF-8'' TEMPLATE = template0' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'coreLib')\gexec

ALTER DATABASE "coreLib" OWNER TO corelib_owner;

\connect "coreLib"

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

CREATE SCHEMA IF NOT EXISTS corelib;

ALTER SCHEMA corelib OWNER TO corelib_owner;

--ALTER TABLE IF EXISTS corelib."coreLib" DROP CONSTRAINT IF EXISTS "userId_fk";


SET default_tablespace = '';
SET default_table_access_method = heap;

 -- below script to create the table 

 DO
$do$
BEGIN
    CREATE TABLE IF NOT EXISTS corelib."versionControl"
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
        CONSTRAINT "coreLibDbversion_pkey" PRIMARY KEY (id)
    );
    COMMENT ON TABLE corelib."versionControl"
        IS 'Created by Dev for storing the db version.';
    ALTER TABLE IF EXISTS corelib."versionControl" OWNER TO corelib_owner;
END
$do$;

DO
$$
BEGIN
    IF EXISTS(select 1 FROM corelib."versionControl" where "majorVersion"='1' and "minorVersion"='0' and "patch"='0' and "build"='0' LIMIT 1)
        THEN
            RAISE NOTICE 'login 1.0.0.0 version already exists. Skipping.';
    ELSE
        BEGIN
			
        CREATE TABLE IF NOT EXISTS corelib.user
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
 
        ALTER TABLE IF EXISTS corelib.user OWNER TO corelib_owner;

        CREATE TABLE IF NOT EXISTS corelib.role
        (
	        id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
	        code text  NOT NULL,
	        description text  NOT NULL,      
	        "dateCreated" timestamp without time zone DEFAULT (timezone('utc', now())),
	        "dateModified" timestamp without time zone DEFAULT (timezone('utc', now())),
	         CONSTRAINT "roles_pkey" PRIMARY KEY (id)
         );
 
        ALTER TABLE IF EXISTS corelib.role OWNER TO corelib_owner;

        CREATE TABLE IF NOT EXISTS corelib.url
        (
	        id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
	        data text  NOT NULL,
	        payload json,
	        description text  NOT NULL,      
	        "dateCreated" timestamp without time zone DEFAULT (timezone('utc', now())),
	        "dateModified" timestamp without time zone DEFAULT (timezone('utc', now())),
	         CONSTRAINT "url_pkey" PRIMARY KEY (id)
         );
 
        ALTER TABLE IF EXISTS corelib.url OWNER TO corelib_owner;

        CREATE TABLE IF NOT EXISTS corelib.query
        (
	        id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
            "urlId" integer,
	        data text  NOT NULL,
	        description text  NOT NULL,      
	        "dateCreated" timestamp without time zone DEFAULT (timezone('utc', now())),
	        "dateModified" timestamp without time zone DEFAULT (timezone('utc', now())),
	         CONSTRAINT "query_pkey" PRIMARY KEY (id)
         );
 
        ALTER TABLE IF EXISTS corelib.query OWNER TO corelib_owner;

          CREATE TABLE IF NOT EXISTS corelib.response
        (
	        id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
            "urlId" integer,
	        data text  NOT NULL,
	        description text  NOT NULL,
            type text,
	        "dateCreated" timestamp without time zone DEFAULT (timezone('utc', now())),
	        "dateModified" timestamp without time zone DEFAULT (timezone('utc', now())),
	         CONSTRAINT "response_pkey" PRIMARY KEY (id)
         );
 
        ALTER TABLE IF EXISTS corelib.response OWNER TO corelib_owner;
			 
         --INSERT SCRIPTS           
          raise notice 'INSERT CHECK or REAL INSERT ON all TABLE';  
        --\echo 'INSERT CHECK or REAL INSERT ON all TABLE'

--roles

    --IF EXISTS ( SELECT FROM pg_tables WHERE schemaname = 'corelib' AND tablename = 'roles' )
    --    THEN
    --    TRUNCATE TABLE ONLY corelib.user RESTART IDENTITY;
 
    --     INSERT INTO corelib.roles(code) VALUES ('admin'),('user');
                 
    --    ELSE
    --        raise notice 'Table corelib."roles" does not exist';
    --END IF;



-- -- re assigning foreign keys 
             
        IF NOT EXISTS (select constraint_name from information_schema.table_constraints where table_schema = 'user' and table_name = 'user' and constraint_name = 'roleId_fk' and constraint_type = 'FOREIGN KEY')   
        THEN   
            ALTER TABLE ONLY corelib.user  
            ADD CONSTRAINT "roleId_fk" FOREIGN KEY ("roleId") REFERENCES corelib.role(id) ON UPDATE CASCADE;   
        ELSE   
            raise notice 'Foreign Key "roleId_fk" on user already exist';   
        END IF;

         IF NOT EXISTS (select constraint_name from information_schema.table_constraints where table_schema = 'user' and table_name = 'query' and constraint_name = 'urlId_fk' and constraint_type = 'FOREIGN KEY')   
        THEN   
            ALTER TABLE ONLY corelib.query  
            ADD CONSTRAINT "urlId_fk" FOREIGN KEY ("urlId") REFERENCES corelib.url(id) ON UPDATE CASCADE;   
        ELSE   
            raise notice 'Foreign Key "urlId_fk" on user already exist';   
        END IF;

        IF NOT EXISTS (select constraint_name from information_schema.table_constraints where table_schema = 'user' and table_name = 'response' and constraint_name = 'responseUrlId_fk' and constraint_type = 'FOREIGN KEY')   
        THEN   
            ALTER TABLE ONLY corelib.response  
            ADD CONSTRAINT "responseUrlId_fk" FOREIGN KEY ("urlId") REFERENCES corelib.url(id) ON UPDATE CASCADE;   
        ELSE   
            raise notice 'Foreign Key "responseUrlId_fk" on user already exist';   
        END IF;

               -- insert version 
            INSERT INTO corelib."versionControl"("majorVersion", "minorVersion", "patch","build","createdDate","dateModified","userModified") VALUES ('1', '0','0','0',timeofday()::TIMESTAMP,timeofday()::TIMESTAMP,'');

     END;
    END IF;
    COMMIT;
END;
$$;

GRANT TEMPORARY ON DATABASE "coreLib" TO PUBLIC;
GRANT CONNECT ON DATABASE "coreLib" TO corelib_owner;
GRANT CONNECT ON DATABASE "coreLib" TO corelib_viewer;
GRANT CONNECT ON DATABASE "coreLib" TO corelib_writer;

GRANT USAGE ON SCHEMA corelib TO corelib_viewer;
GRANT ALL ON SCHEMA corelib TO corelib_owner;
GRANT ALL ON SCHEMA corelib TO corelib_writer;

GRANT ALL ON TABLE corelib.user TO corelib_owner;
GRANT ALL ON TABLE corelib.role TO corelib_owner;
GRANT ALL ON TABLE corelib.url TO corelib_owner;
GRANT ALL ON TABLE corelib.query TO corelib_owner;
GRANT ALL ON TABLE corelib.response TO corelib_owner;

GRANT SELECT ON TABLE corelib."versionControl" TO corelib_viewer;
GRANT SELECT ON TABLE corelib.user TO corelib_viewer;
GRANT SELECT ON TABLE corelib.role TO corelib_viewer;
GRANT SELECT ON TABLE corelib.url TO corelib_viewer;
GRANT SELECT ON TABLE corelib.query TO corelib_viewer;
GRANT SELECT ON TABLE corelib.response TO corelib_viewer;

GRANT SELECT,INSERT,UPDATE ON TABLE corelib."versionControl" TO corelib_writer;
GRANT SELECT,INSERT,UPDATE ON TABLE corelib.user TO corelib_writer;
GRANT SELECT,INSERT,UPDATE ON TABLE corelib.role TO corelib_writer;
GRANT SELECT,INSERT,UPDATE ON TABLE corelib.url TO corelib_writer;
GRANT SELECT,INSERT,UPDATE ON TABLE corelib.query TO corelib_writer;
GRANT SELECT,INSERT,UPDATE ON TABLE corelib.response TO corelib_writer;

REVOKE CONNECT,TEMPORARY ON DATABASE "coreLib" FROM PUBLIC;

\echo 'SCRIPT COMPLETE FOR user DB'
