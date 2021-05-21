# CoreCodeCamp

The purpose of this project is to create a project for running a Code Camp 
or other small, local development event. This project will be used for the
Atlanta Code Camp starting in 2016.

## Technologies

The project uses the following technologies:
* .NET 5
* Bootstrap 4.5
* Vue 2
* TypeScript
* Azure Blog Storage

## Running locally

You must have [SQL Server Express LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-2017) installed and started.

1. clone the repo
1. run `npm install` from the `src/CoreCodeCamp` folder
1. edit `src/CoreCodeCamp/appsettings.json`
  - set `SeedDatabase` to `true`
  - set `IterateDatabase` to `true`
1. run the app with VS Code or Visual Studio or `dotnet run`
1. restore the settings above back to `false` to avoid re-creating the database from scratch each time
