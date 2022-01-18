sudo apt -q install -y sqlite3 #na wypadek braku sqlite
sqlite3 Sqlite/password.db ".read Sqlite/initdb.sql" #czytanie schemy bazy z pliku
