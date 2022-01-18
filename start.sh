echo "Initializing sqlite3 database..."
sh Sqlite/sqlite.sh
echo "Finished initializing database!"
echo "Initializing web app docker image..."
sudo docker build 
