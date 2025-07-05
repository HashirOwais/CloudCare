#For ENV vars for Connection strung 

```
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=devdb;User=dev;Password=devpw;"
  }
}
```
Use this in the appsettting.dev json file 

```
export ConnectionStrings__Default="Server=prod;Database=proddb;User=prod;Password=secr3t;"
```
Use this in the prod enviroemnt 

BUT in the code jsut do this 

```
var conn = builder.Configuration.GetConnectionString("Default");
// This will use the environment variable if present, else falls back to the dev JSON.
```
