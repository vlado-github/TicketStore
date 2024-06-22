# TicketStore

## Setup Solution
1. Download and install .net SDK 8
2. Clone repository
3. Build and Run TicketStore.API:https
4. Browse https://localhost:7113/swagger/index.html

## Setup Database

### Update database to recent schema
Use following command from root repository directory (**TicketStore**):

Windows
```bash
dotnet ef database update --project .\TicketStore.DAL\ --startup-project .\TicketStore.API\
```

MacOS/Linux
```bash
export ASPNETCORE_ENVIRONMENT='Local' && dotnet ef database update --project ./TicketStore.DAL/ --startup-project ./TicketStore.API/
```

For different environment in build pipeline use this:
 
Windows
```bash
dotnet ef database update --project .\TicketStore.DAL\ --startup-project .\TicketStore.API\ -- --environment Development
```

MacOS/Linux
```bash
export ASPNETCORE_ENVIRONMENT='Development' && dotnet ef database update --project ./TicketStore.DAL/ --startup-project ./TicketStore.API/ -- --environment Development
```

### Create Database migration script
Use following command from root repository directory (**TicketStore**):

Windows
```bash
dotnet ef migrations add <MigrationTitle> --project .\TicketStore.DAL\ --startup-project .\TicketStore.API\
```

MacOS/Linux
```bash
dotnet ef migrations add <MigrationTitle> --project ./TicketStore.DAL/ --startup-project ./TicketStore.API/
```