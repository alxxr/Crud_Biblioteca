create table autor
(
    id     serial
        primary key,
    nombre varchar(200) not null
);

alter table autor
    owner to "user";

create table book
(
    id       serial
        primary key,
    titulo   varchar(200) not null,
    autor_id integer      not null
        references autor
);

alter table book
    owner to "user";


