# battleship-game
"Battle Ship" game project to demo React, Redux, .NET Core, SignalR, WepApi in docker and also some approaches like JWT authentication, refresh tokens etc.
The game is running on http://battle-ship.xyz

## server-app
Server app uses Automapper, SignalR and Swagger, MongoDB (optional).
Show some common pieces for modern applications like:
 - JWT authentication and Refresh Tokens.
 - .Net Core BackgroundService class usage.
 - Switching Storage based on configuration (InMemory, MongoDb)

### requirements
Requires .NET Core 2.2.3 and Visual Studio 15.9.9 to run the project. 
If MongoDB will be used as storage - requires Mongo Server v4.
Preconfigured to use Mailtrap.io free account to get emails in development mode.

### storage
By default InMemory storage is used. It does not require any DB to be installed, but the data are not persisted.
This is for easy start. To switch to persistent storage - specify MongoDb in appsettings.json.

### starting from Visual Studio
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
Modify "docker.run.ps1" script. For example if you  forward port 80 from docker to port 8091 on docker host and your client app is runing on http://localhost:8092 command line will look like
```
docker run -d -p 8091:80 --env Cors__AllowedOrigins=http://localhost:8092 olmelabs/battleshipapi
```
Open powershell and run modified script. This will run dettached process. 
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
User accounts are currently disabled, but login credentials for demo user: **user@domain.com / password**

## Release notes 
## 0.3.5:
 - added localization
 - added move "traffic lights"
## 0.3.4:
 - implemented multiplayer game mode
 - improved ships layout experience
 - migrated server to latest .NET Core 2.2.3
 - migrated client to React 16.8
 - many other bugfixes and improvements
## 0.3.3:
 - backend migrated to .netcore 2.2.1.
 - docker image updated
 - all other backend packages updated to latest versions.
 - client migrated to reat 16.7, react-redux 6, signal r 1.1.0
 - updated client unit tests to work with updated packages
 - fixed CORS on backend to match signalr changes
