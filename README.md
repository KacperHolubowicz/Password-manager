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

# Screeny aplikacji
### Strona główna aplikacji
  ![strona-glowna](https://user-images.githubusercontent.com/93927311/150705251-bc6e7c3e-b549-4758-bd75-6744f5025004.png)
  
  ### Widok formularza rejestracji nowego użytkownika
  ![rejestracja](https://user-images.githubusercontent.com/93927311/150705292-40b8e070-2ada-454f-82cc-c542f426b45b.png)

  ### Aplikacja wymaga od swych użytkowników silnych haseł - za takie można uznać hasła o co najmniej 8 znakach, w tym jednym znaku dużej litery, jednej cyfrze i jednym znaku specjalnym
![slabe-haslo](https://user-images.githubusercontent.com/93927311/150705347-2cffc006-946d-4794-b31d-1cefe59aa8b0.png)

 ### Widok logowania
  ![logowanie](https://user-images.githubusercontent.com/93927311/150705377-c8853dc7-6504-4ab2-a476-480e003268e7.png)

  ### Aplikacja zlicza wszystkie próby logowania z danego adresu IP - użytkownik ma maksymalnie 5 prób na zalogowanie się
  ![logowanie-zle](https://user-images.githubusercontent.com/93927311/150705435-d6d012f1-040c-4931-a3c8-08473c84bd55.png)
  
  ### Po pięciu nieudanych próbach nakładana jest blokada na dany adres IP - długość blokady rośnie liniowo wraz z ich ilością
  ![logowanie-blokada1](https://user-images.githubusercontent.com/93927311/150705493-e0df5247-0d12-43a8-8c56-1c270caac2da.png)
![logowanie-blokada2](https://user-images.githubusercontent.com/93927311/150705502-228765e5-b0af-4eda-a490-b047278284a6.png)

### Ekran funkcji "zapomniałem hasła" - sprawdzane jest jedynie istnienie danego adresu email w bazie danych i wyświetlenie komunikatu o rzekomym wysłaniu linku resetu hasła na ten adres
  ![forgot-password](https://user-images.githubusercontent.com/93927311/150705589-c028bbc8-e5e1-4312-b25b-30a5970f463d.png)

  ### Ekran strony głównej po zalogowaniu się - w przypadku zalogowania się z nowego urządzenia (to jest inny rodzaj urządzenia, inna przeglądarka i inny system operacyjny) jest wyświetlany odpowiedni komunikat oraz te urządzenie jest zapisywane
  ![strona-glowna-zalogowany](https://user-images.githubusercontent.com/93927311/150705659-38681d73-34fc-4999-9d05-b6c9e1d95fc2.png)

  ### Lista wszystkich urządzeń zapisanych przez danego użytkownika jest widoczna w widoku Devices
  ![urzadzenia](https://user-images.githubusercontent.com/93927311/150705710-5baf4a38-d536-4e32-82fb-116c9f4d9dd0.png)
  
  ### Lista zapisanych wszystkich haseł użytkownika
  ![hasla1](https://user-images.githubusercontent.com/93927311/150705724-213a7593-a2aa-4ba9-a68d-94b8eac5c83e.png)

### Widok dodania nowego hasła, operacja ta wymaga podania prawidłowego master passworda
  ![dodaj-haslo](https://user-images.githubusercontent.com/93927311/150705749-fd3d7391-f2b8-41b5-8aab-9db7d0b1c388.png)

  ### Lista haseł tuż po dodaniu nowego - jak widać wszystkie hasła można edytować, wyświetlić oraz usunąć. Każda z tych operacji wymaga podania master passworda
  ![hasla2](https://user-images.githubusercontent.com/93927311/150705790-1050849a-6e33-4cd2-9c06-3da416b84d1f.png)
  
  ### Widok wyświetlenia hasła
  ![pokaz-haslo](https://user-images.githubusercontent.com/93927311/150705841-e1c212b3-9ff1-4d33-acbc-9815a757b9c6.png)
  ![pokazane-haslo](https://user-images.githubusercontent.com/93927311/150705823-24c65d3c-db90-45ce-b807-2f31d7263f88.png)


  # Szczegóły implementacyjne
  ### Szyfrowanie haseł do kont oraz master passwordów
  Za tworzenie funkcji skrótu dla tych haseł odpowiada klasa implementująca metody z interfejsu ISecretsService. Podczas rejestracji nowego konta generowana jest specjalnie dla użytkownika sól, która jest przechowywana w tabeli.
Do tworzenia funkcji skrótu wykorzystany jest algorytm Pbkdf2 wykorzystujący HMAC-SHA512.
  ### Szyfrowanie haseł do serwisów
  Ponieważ użytkownik zapisuje hasła do różnych serwisów w celu ich późniejszego wyświetlenia, należało zastosować kryptografię symetryczną. W tym przypadku hasła są szyfrowane algorytmem AES w trybie CBC, w którym wektor inicjalizujący IV jest generowany specjalnie do danego hasła, zaś kluczem jest master password użytkownika.
  ### Logowanie się
  Podstawą jest, by nie wyświetlać dokładnej przyczyny odmowy uwierzytelnienia podanych danych logowania. By utrudnić jednak ataki brute-force zastosowano zaróœno celowe "usypianie" aplikacji jak i kontrolowanie liczby prób logowania z danej maszyny.
  ### Protokół SSL
  Aplikacja jest dostosowana do tzw. reverse proxy w postaci serwera Nginx oraz korzysta z samopodpisanych (a więc nieuznawanych za bezpieczne przez przeglądarki) certyfikatów.
  ### SQL Injection / CSRF
  Dapper, jako mikro ORM, wymaga pisania skryptów SQL, co czyni go teoretycznie mniej bezpiecznym od bardziej zautomatyzowanych frameworków, jak np Entity Framework Core. Dapper daje jednak możliwość parametryzowania zapytań, co daje ochronę przez atakiem SQL Injection (więcej pod https://dapper-tutorial.net/parameter-anonymous)
  W celu ochrony przed atakami CSRF w aplikacji stosowane są tzw. Antiforgery tokeny (są automatycznie wykorzystywane przez formularze w szablonie widoków Razor Pages). Ponieważ tokeny te w środowisku produkcyjnym wymagają przechowywania klucza (np w bazie Redis lub w Azure Blob Storage) aplikacja działa poprawnie wyłącznie w środowisku developerskim
  ### Content-Security-Policy
  Serwer zwraca nagłówek Content-Security-Policy stanowiący barierę przed atakami XSS
  ### Weryfikacja urządzeń
  W celu weryfikacji urządzeń wykorzystano pakiet wangkanai/Detection (https://github.com/wangkanai/Detection). Urządzenia są wykrywane poprzez parse'owanie user agenta, co nie powinno wpływać negatywnie na bezpieczeństwo aplikacji
