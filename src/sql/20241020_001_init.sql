-- SELECT session_user, current_user;
-- SELECT * FROM pg_user;
-- SELECT * FROM pg_roles;

-- on Azure PostgreSQL you need to log with admin user and create those databases and users
create DATABASE obs01prices
create DATABASE obs01products
create DATABASE obs01stock

create user obs_prices
create user obs_products
create user obs_stock
grant all privileges on database obs01prices to obs_prices;
grant all privileges on database obs01products to obs_products;
grant all privileges on database obs01stock to obs_stock;
--alter user obs_prices with encrypted password 'SOME_PASSWORD';
--alter user obs_products with encrypted password 'SOME_PASSWORD';
--alter user obs_stock with encrypted password 'SOME_PASSWORD';

-- for each of those grants you need to log directly to the newly created databases end execure on each DB separately
-- obs_prices
GRANT ALL privileges ON SCHEMA public TO obs_prices;

-- obs_products
GRANT ALL privileges ON SCHEMA public TO obs_products;

-- obs_stock
GRANT ALL privileges ON SCHEMA public TO obs_stock;