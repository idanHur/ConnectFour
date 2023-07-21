# Connect4Game Server

This project is an ASP.NET Core web application server for Connect4Game. It serves as the backbone for game management and player interactions. 

## Main Components

1. **Server**: This is the ASP.NET Core web application that provides the following functionalities:
    - User registration: New players can register via the web interface.
    - Data presentation: Allows users to view various data retrieved via database queries.
    - Database management: Provides an interface for deleting and editing player and game data in the database.
    - Animation: Supports falling game coins animation for enhanced user interaction.
2. **Game Manager**: Manages the players and facilitates the connection to the Entity Framework SQL Server database.
3. **Connect4Game**: Contains the core game logic for Connect4Game.

The server communicates with the client through a Web API. Each client is a player with a user interface for the game board. The server handles the game logic and opponent AI.

## Getting Started

### Prerequisites
- .NET Core 3.1 or higher
- SQL Server
- (Optional) SQL Server Management Studio

### Setup
1. Make sure you have .NET Core 3.1 or higher installed on your machine.
2. Install SQL Server and optionally, SQL Server Management Studio for easier database management.
3. Clone the repository to your local machine.
4. Navigate to the project directory.
5. Run `dotnet restore` to restore the necessary NuGet packages.
6. Update the connection string in your `appsettings.json` file with your SQL Server details.
7. Run `dotnet ef database update` to create your database schema.
8. Run `dotnet run` to start the server.

### Database
To update the database tables when changing one of the entity classes, perform the following in the terminal when in the server directory:
1. `dotnet ef migrations add <New name for the migrations> --project ../GameManager/GameManager.csproj --startup-project Server.csproj`
2. `dotnet ef database update --project ../GameManager/GameManager.csproj --startup-project Server.csproj`

## Code Practices

This project aims to follow good coding standards and practices. One of the key implementations is the use of partial views to avoid code duplication. 

In the context of MVC architecture, views often share code with other views. To DRY (Don't Repeat Yourself) the code, common code was extracted into partial views. These partial views are then used in multiple other views, enhancing maintainability and promoting code reusability.
