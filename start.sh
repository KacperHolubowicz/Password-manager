# Skrypt pisany pod linux Mint, zatem powinien zadzialac rowniez na ubuntu
# Skrypt uruchamiajacy baze sqlite w katalogu /tmp

echo "Initializing sqlite3 database..."
sh Sqlite/sqlite.sh
echo "Finished initializing database!"

sudo apt install dotnet-sdk-5.0 # sdk frameworka .net 5.0, POWINIEN zawierac runtime do asp.net core
dotnet run --project WebApp/WebApplication/WebApplication.csproj