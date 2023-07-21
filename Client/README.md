# Connect4Game Client

This project is a WPF application serving as the client for the Connect4Game. It provides a user interface for the game and connects to the server using a Web API. The game board is dynamically created and features animations for an engaging user experience.

## Main Components

1. **Client Application**: The primary WPF application that presents the user interface and game board for the Connect4Game.
2. **Game Logic Client**: A class library that houses entities related to the game and contains the data layer. The data layer interacts with a SQL Server database via Entity Framework Core to store and retrieve game and player information. Although the main game logic is handled by the server, this component keeps track of the game on the client side.

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
7. Run the WPF application.

### Database
To update the database tables when changing one of the entity classes, perform the following in the Package Manager Console in Visual Studio:
1. `Add-Migration <New name for the migrations> -Project GameLogicClient`
2. `Update-Database -Project GameLogicClient`
