## Migration 

- temp: run 
```
dotnet ef migrations bundle --self-contained -r linux-x64
```
- then in dc-prd run it this way; 
```bash
./efbundle --connection "$CONNECTION_STRING" --verbose
```

- In future this will be automated by using GitOps principle. For now this is how i do it 