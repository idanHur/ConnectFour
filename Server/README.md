# Connect4Game Server

This project is an ASP.NET Core web application server for Connect4Game. It consists of three main components:

1. **Server**: This is the ASP.NET Core web application that hosts the website for user registration and allows users to see the database queries.
2. **Game Manager**: This manages the players and is responsible for the connection to the Entity Framework SQL Server database.
3. **Connect4Game**: This contains the game logic for Connect4Game.

The server uses a Web API to communicate with the client. Each client is a player with a user interface for the game board. The server handles the logic and the opponent AI.

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

