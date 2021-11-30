### Entity Framework Core Demo App
## Build DB Docker image
This command also creates a database with the name "SamuraiAppData"
```
docker build -t <docker image name> .
```
## Start DB
```
docker run -p 1433:1433 -d <docker image name>
```

## Install EF Core CLI Tool
```
dotnet tool install --global dotnet-ef
```

## Update DB
```
dotnet ef database update --verbose 
```