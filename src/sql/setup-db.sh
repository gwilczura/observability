# execute like this:
# ./setup-db.sh '[db-host-name]' '[sqladmin-password]' '[domain-password-prefix]'
export PGHOST=$1
export PGDATABASE=postgres
export PGPORT=5432
export PGUSER=sqladmin
export PGPASSWORD=$2

PRICES_PASS="${3}1";
PRODUCTS_PASS="${3}2";
STOCK_PASS="${3}3";

psql -a -w -f 01-setup-db-and-user.sql -v dbName=db_prices -v userName=usr_prices -v userPassword=$PRICES_PASS
psql -a -w -f 01-setup-db-and-user.sql -v dbName=db_products -v userName=usr_products -v userPassword=$PRODUCTS_PASS
psql -a -w -f 01-setup-db-and-user.sql -v dbName=db_stock -v userName=usr_stock -v userPassword=$STOCK_PASS

PGDATABASE=db_prices
psql -a -w -f 02-grant-schema-access.sql -v userName=usr_prices
PGDATABASE=db_products
psql -a -w -f 02-grant-schema-access.sql -v userName=usr_products
PGDATABASE=db_stock
psql -a -w -f 02-grant-schema-access.sql -v userName=usr_stock