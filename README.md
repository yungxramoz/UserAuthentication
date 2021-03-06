# UserAuthentication

.NET 5, User Authentication Web API

## Technologie

- .NET 5
- EF Core 5.0.3
- AutoMapper 10.1.1
- JWT Bearer 5.0.3
- SQL Server

## Projekt setup

In der Datei "appsettings.Development.json" den ConnectionString UserDatabase den Server anpassen, dass auf die korrekte Datenbank verbunden wird.

```json
"ConnectionStrings": {
    "UserDatabase": "Server=SqlServer;Database=UserDB;Trusted_Connection=True;"
  },
...
```

## User Manual

1. /api/User/register aufrufen um einen neuen Benutzer anzulegen
2. /api/User/authenticate mit gültigen Credentials abfragen um ein JWT Token zu bekommen
3. Mit dem Bearer Token als Authentifizierung können nun alle anderen API Schnittstellen genutzt werden

`Detailierte Dokumentation zur API ist im Swagger UI zu finden, das beim starten geöffnet wird (/swagger/index.html)`
