CREATE TABLE "Users" (
    "Id" uuid default gen_random_uuid() not null primary key,
    "Email" text not null unique,
    "UserName" text not null unique,
    "Gender" integer not null,
    "Role" integer not null,
    "DateOfBirth" timestamp without time zone not null,
    "Password" text not null,
    "LastLoginDate" timestamp without time zone,
    "CreationDate" timestamp without time zone not null   
);