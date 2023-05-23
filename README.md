# Password-manager
Project was created as a part of the "Data protection in information systems" course

# Running
It is recommended to use start.sh script for the project initialization, as it:
1. Creates database in /tmp/ directory
2. Copies ssl certificate, its key and configuration file "Default"
3. Runs web application
  
After the initialization the application can be run with the command **dotnet run --project "path"**  with  **"path"** being a path to the **WebApplication.csproj** file 
  
# Technology stack
  
Project was written in the .NET 5 platform in the ASP.NET Core MVC framework, Sqlite3 was the database of choice along with the micro ORM Dapper
  
# Application screens
### Application main page
  ![strona-glowna](https://user-images.githubusercontent.com/93927311/150705251-bc6e7c3e-b549-4758-bd75-6744f5025004.png)
  
  ### New user registration form view
  ![rejestracja](https://user-images.githubusercontent.com/93927311/150705292-40b8e070-2ada-454f-82cc-c542f426b45b.png)

  ### Application demands strong passwords from its users, which consist of passwords containing at least 8 characters, including one uppercase letter, one digit and one special character
![slabe-haslo](https://user-images.githubusercontent.com/93927311/150705347-2cffc006-946d-4794-b31d-1cefe59aa8b0.png)

 ### Signing in view
  ![logowanie](https://user-images.githubusercontent.com/93927311/150705377-c8853dc7-6504-4ab2-a476-480e003268e7.png)

  ### Application counts all sign in attempts coming from a given IP address - the user has at most 5 attempts to sign in
  ![logowanie-zle](https://user-images.githubusercontent.com/93927311/150705435-d6d012f1-040c-4931-a3c8-08473c84bd55.png)
  
  ### After the five failed attempts an IP address gets blocked - block's length grows linearly along with the amount
  ![logowanie-blokada1](https://user-images.githubusercontent.com/93927311/150705493-e0df5247-0d12-43a8-8c56-1c270caac2da.png)
![logowanie-blokada2](https://user-images.githubusercontent.com/93927311/150705502-228765e5-b0af-4eda-a490-b047278284a6.png)

### "I forgor password" mocked functionality view - only email's existence is checked 
  ![forgot-password](https://user-images.githubusercontent.com/93927311/150705589-c028bbc8-e5e1-4312-b25b-30a5970f463d.png)

  ### Main page view after signing in - in the case of signing in from a new device (different device type, different browser or different OS) a relevant message is shown and this device is saved in the system
  ![strona-glowna-zalogowany](https://user-images.githubusercontent.com/93927311/150705659-38681d73-34fc-4999-9d05-b6c9e1d95fc2.png)

  ### List of all devices saved by the user is shown in the **Devices** view
  ![urzadzenia](https://user-images.githubusercontent.com/93927311/150705710-5baf4a38-d536-4e32-82fb-116c9f4d9dd0.png)
  
  ### List of all passwords saved by the user
  ![hasla1](https://user-images.githubusercontent.com/93927311/150705724-213a7593-a2aa-4ba9-a68d-94b8eac5c83e.png)

### Adding new password view, this operation requires providing a correct master password
  ![dodaj-haslo](https://user-images.githubusercontent.com/93927311/150705749-fd3d7391-f2b8-41b5-8aab-9db7d0b1c388.png)

  ### Password list after adding a new one - all passwords can be edited, shown and deleted. All of these operations require providing master password
  ![hasla2](https://user-images.githubusercontent.com/93927311/150705790-1050849a-6e33-4cd2-9c06-3da416b84d1f.png)
  
  ### Show password view
  ![pokaz-haslo](https://user-images.githubusercontent.com/93927311/150705841-e1c212b3-9ff1-4d33-acbc-9815a757b9c6.png)
  ![pokazane-haslo](https://user-images.githubusercontent.com/93927311/150705823-24c65d3c-db90-45ce-b807-2f31d7263f88.png)


  # Implementation details
  ### Master passwords and account passwords hashing
Class implementing the ISecretsService interface is responsible for creating a hash for given password. During registration of a new account a dedicated salt is generated for the user and it is stored in the database table.
Algorithm Pbkdf2 using HMAC-SHA512 is used for creating hashes.
  ### Ciphering service passwords
  Because of the fact that user saves passwords to different services for the sake of reading them later, a symmetric cryptography was used. In this case passwords are ciphered with AES algorithm in CBC mode, which requires an initialization vector IV to be generated just for given password with the key being user's master password. 
  ### Signing in
  One of the basic rules is not to display a detailed cause of authentication failure. In order to make brute-force attacks less effective, both "putting app to sleep" and controlling signing in attempts were used.
  ### SSL protocol
  Application is adapted to the Nginx reverse proxy and it uses self-signed certificates (which aren't considered safe by the browsers)
  ### SQL Injection / CSRF
  Dapper, as a micro ORM, requires SQL queries to be manually written, which, theoretically, makes it less safe than more automated framework, such as Entity Framework Core. However, Dapper allows parameterized queries, which protect from SQL Injection attacks (for more info https://dapper-tutorial.net/parameter-anonymous).
  So called Antiforgery tokens are used as a defence from CSRF attacks (they are automatically used by the Razor Pages forms), although these tokens require storing the key (for example in the Redis database or in Azure Blob Storage) in the production environment, which is why the application only works correctly in the development environment. 
  ### Content-Security-Policy
  Server returns Content-Security-Policy header that protects from XSS attacks.
  ### Device verification
  In order to verify devices a wangkanai/Detection (https://github.com/wangkanai/Detection) package was used. Devices are detected by parsing the useragent, which should not have any negative impact on the application security.
