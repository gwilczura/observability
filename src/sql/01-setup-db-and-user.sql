create DATABASE :dbName;

create user :userName;

alter user :userName with encrypted password :'userPassword';

grant all privileges on database :dbName to :userName;