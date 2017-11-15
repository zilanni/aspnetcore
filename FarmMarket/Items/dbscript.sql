CREATE SCHEMA `farmmarket` DEFAULT CHARACTER SET utf8 ;

CREATE TABLE user_User(
    Id varchar(50) not null primary key,
    UserName varchar(50)  not null,
    NormalizedUserName varchar(50) not null,
    Email varchar(100) null,
    NormalizedEmail varchar(100) null,
    PasswordHash varchar(200) null,
    SecurityStamp varchar(200) null,
    ConcurrencyStamp varchar(200),
    PhoneNumber varchar(50) null ,
    PhoneNumberConfirmed bit not null,
    TwoFactorEnabled bit not null,
    LockoutEnabled bit not null,
    LockoutEnd datetime null,
    AccessFailedCount int not null default 0,
    EmailConfirmed bit not null default 0
) ;

alter table user_User add unique key uk_user_User_NormalizedUserName(NormalizedUserName);
alter table user_User add unique key uk_user_NormalizedEmail(NormalizedEmail);

CREATE TABLE user_UserRole(
    Id int not null primary key auto_increment,
    UserId varchar(50) not null,
    RoleId varchar(50) not null
);

alter table user_UserRole add unique key uk_user_User_UserId_RoleId(UserId,RoleId);
alter table user_UserRole add index ix_user_User_RoleId(RoleId); 

