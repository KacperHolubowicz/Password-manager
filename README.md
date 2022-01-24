# Password-manager
Projekt wykonany w ramach części laboratoryjnej przedmiotu Ochrona danych w systemach informatycznych

# Uruchamianie
Do inicjalizacji projektu najlepiej wykorzystać skrypt start.sh, który:
1. Tworzy bazę danych w katalogu /tmp/
2. Kopiuje certyfikat ssl, jego klucz oraz plik konfiguracyjny Default
3. Uruchamia aplikację webową

Po inicjalizacji aplikację można uruchamiać poleceniem dotnet run --project <path>, gdzie <path> jest ścieżką do pliku WebApplication.csproj

# Stack
Projekt został napisany w platformie .NET 5 we frameworku ASP.NET Core MVC, jako baza danych została wybrana baza Sqlite3, zaś ORM służącym ich wzajemnej komunikacji został mikro ORM Dapper
