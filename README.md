# battleship-game
"Battle Ship" game project to demo react, redux, netcore, signalr, wep api in docker and also some approaches like JWT authentication, refresh tokens etc.

## server-app
Server app uses Automapper, SignalR and Swagger, MongoDB (optional).
Shows:
 - JWT authentication and Refresh Tokens.
 - .Net Core 2.1 BackgroundService class usage.
 - Switching Storage based on configuration (InMemory, MongoDb)

### requirements
Requires .NET Core 2.1.3 and VS2017 15.8 to run the project. 
Requires Mailtrap.io free account to get sample emails.
If MongoDB will be used as storage - requires Mongo Server v4.

### storage
By default InMemory storage is used. It does not require any DB to be installed, but the data are not persisted.
This is for easy start. To switch to persistent storage - specify MongoDb in appsettings.json.

### starting from VS 2017
Open solution in VS2017. 
Create user secret file in VS. Sample data are placed in "secret.json.template" 
If you do not have docker running, you will receive error message - just disregard it.
Run (F5) it from VS.
If everything is ok, you will see api swagger on http://localhost:63354/swagger

### starting from command line
Navigate to solution directory, run following in powershell command line.
```
dotnet restore olmelabs.battleship.api.sln 
dotnet build olmelabs.battleship.api.sln 
dotnet run --project olmelabs.battleship.api\olmelabs.battleship.api.csproj
```
If everything is ok, you will see api swagger on http://localhost:63354/swagger

### getting server image from docker.hub
get image from docker hub to you docker instance
```
docker pull olmelabs/battleshipapi
```
However it has my mailtrap.io account tokens preconfigured. You can test api, but i may reset token at any moment.

### starting in docker on windows platform
Rename file "appsettings.Production.json.template" to "appsettings.Production.json" and add mailtrap.io credentilas to get your development emails.
Run
```
docker-compose build
```
Open powershell and run  
```
.\docker.run.ps1
```
or to start dettached from docker compose
```
docker-compose up -d
```
You should see swagger on http://localhost:8091/swagger

## client-app
Client app and uses React, Redux, SignalR. From tools perspective - Babel, ESLint, Webpack.
Shows:
 - JWT authentification and Refresh tokens approach. 
 - chained promises approach and async \ await

To start in dev mode
Adjust server url in src\helpers\api.config.dev.js 
```
npm install
npm run
```
To start in prod mode
Adjust server url in src\helpers\api.config.prod.js 
```
npm install
npm run build
```
Login credentials for demo user: **user@domain.com / password**
## Recent changes:
 - Added user accounts flow. Login, Registration, Reset Paswsord
 - Added quick board generation.
 - Added simple user accounts to demo protected routes and further features
 - Added MongoDB as possible storage.
 - Updated server-app from netcore2.1.0 to netcore 2.1.2
 - Added JWT authentification and Refresh tokens.
 - Updated server-app from netcore2.0 to netcore 2.1 
 - Updated client app to use signalr 1.0.0
 - Added more friendly ship placement flow.

## Roadmap:
 - Add client side board validation 
 - Add client tests
 - ~~Add MongoDb as persistent stoage~~
 - ~~Add user accounts to proceed with collecting game statistics and other cool staff.~~
