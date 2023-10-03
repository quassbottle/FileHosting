CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS file_meta (
    id uuid NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
    size bigint,
    name text,
    type varchar(24),
    data_id uuid references file_data(id)
);

CREATE TABLE IF NOT EXISTS file_data (
    id uuid NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
    data bytea,
    meta_id uuid NOT NULL REFERENCES file_meta(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS file_url (
    id uuid  NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
    meta_id uuid REFERENCES file_meta(id) ON DELETE CASCADE
);