# Connect4Game Project

This project is a complete implementation of the Connect4Game with a WPF client and an ASP.NET Core server. The game is split into two main components, the client and the server, each encapsulating its own specific functionality and responsibilities.

## Table of Contents
- [Main Components](#main-components)
- [Prerequisites](#prerequisites)
- [Setup](#setup)
- [Setting Up the Server](#setting-up-the-server)
- [Setting Up the Client](#setting-up-the-client)
- [Updating the Database](#updating-the-database)
- [Code Practices](#code-practices)
  
## Main Components

### Server (ASP.NET Core Web Application)

The server is the backbone for game management, game logic, player interactions, and AI opponent management. Its main functionalities include:

- **User registration**: New players can register via the web interface.
- **Data presentation**: Allows users to view various data retrieved via database queries.
- **Database management**: Provides an interface for deleting and editing player and game data in the database.
- **Animation**: Supports falling game coins animation for enhanced user interaction.
- **Game Logic**: Enforces the rules of the Connect4 game, validating player moves, and managing game progress.
- **AI Player**: Implements an artificial intelligence algorithm that plays against the player, making decisions based on the current state of the game.
- **JWT Authentication**: Implemented using the System.IdentityModel.Tokens.Jwt namespace to ensure registered clients.
- **Running with Docker**: The server can be containerized and run with Docker.
- **Running with Kubernetes**: Deployment configurations for Kubernetes are available for scaling and deploying the application across a cluster of machines.

### Client (WPF Application)

The client provides a user interface for the game and connects to the server using a Web API. It presents the game board dynamically and features animations for an engaging user experience.

## Prerequisites

- .NET Core 3.1 or higher
- SQL Server
- (Optional) SQL Server Management Studio
- Docker Desktop (If you want to run the server using Docker)
- Minikube or a Kubernetes cluster setup, kubectl (If you want to run the server with Kubernetes)

## Setup

1. Make sure you have .NET Core 3.1 or higher installed on your machine.
2. Install SQL Server and optionally, SQL Server Management Studio for easier database management.
3. Clone the repository to your local machine.
4. Navigate to the project directory.
5. Run `dotnet restore` to restore the necessary NuGet packages.
6. Update the connection string in your `appsettings.json` file with your SQL Server details.

## Setting Up the Server

For detailed setup instructions, refer to the [server readme](server/README.md).

**Note:** Depending on how you run the server, you may need to update the server's address in the client configuration.


## Setting Up the Client

1. Navigate to the client project directory.
2. Run the WPF application.

## Updating the Database

To update the database tables when changing one of the entity classes, perform the following:

### For the Server

In the terminal when in the server directory:

1. `dotnet ef migrations add <New name for the migrations> --project ../GameManager/GameManager.csproj --startup-project Server.csproj`
2. `dotnet ef database update --project ../GameManager/GameManager.csproj --startup-project Server.csproj`

### For the Client

In the Package Manager Console in Visual Studio:

1. `Add-Migration <New name for the migrations> -Project GameLogicClient`
2. `Update-Database -Project GameLogicClient`

## Code Practices

This project aims to follow good coding standards and practices. One of the key implementations is the use of partial views to avoid code duplication. Both MVC and MVVM patterns have been used to ensure a clear separation of concerns, with Controllers and ViewModels ensuring the smooth management of data between the views and the model.

In the context of MVC architecture, views often share code with other views. To DRY (Don't Repeat Yourself) the code, common code was extracted into partial views. These partial views are then used in multiple other views, enhancing maintainability and promoting code reusability.

Through this project, we aim to provide a seamless and interactive gaming experience for all Connect4Game enthusiasts. Enjoy the game!

**Note**: Make sure to replace `<New name for the migrations>` with the name you want to give to the new migration.
