sudo apt -q install -y sqlite3 #na wypadek braku sqlite
sqlite3 /tmp/password.db ".read Sqlite/initdb.sql" #czytanie schemy bazy z pliku
