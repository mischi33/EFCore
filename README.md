### Entity Framework Core Demo App
## Start DB
```
docker build -t <docker image name> .
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