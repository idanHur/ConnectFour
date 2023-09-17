# Connect4Game Server

This project is an ASP.NET Core web application server for Connect4Game. It serves as the backbone for game management and player interactions. 


- [Main Components](#main-components)
- [JWT Authentication](#jwt-authentication)
- [Database](#database)
- [Getting Started without Docker](#getting-started-without-docker)
- [Running with Docker](#running-with-docker)
- [Running with Visual Studio](#running-with-visual-studio)
- [Running with Kubernetes](#running-with-kubernetes)
- [Code Practices](#code-practices)


## Main Components

1. **Server**: This is the ASP.NET Core web application that provides the following functionalities:
    - User registration: New players can register via the web interface.
    - Data presentation: Allows users to view various data retrieved via database queries.
    - Database management: Provides an interface for deleting and editing player and game data in the database.
    - Animation: Supports falling game coins animation for enhanced user interaction.
2. **Game Manager**: Manages the players and facilitates the connection to the Entity Framework SQL Server database.
3. **Connect4Game**: Contains the core game logic for Connect4Game.

The server communicates with the client through a Web API. Each client is a player with a user interface for the game board. The server handles the game logic and opponent AI.

## JWT Authentication
To ensure that only registered clients can interact with the server, JWT (JSON Web Token) authentication has been implemented using the `System.IdentityModel.Tokens.Jwt` namespace. Here's how it works:

- **Upon successful login**, the server generates a JWT for the client.
- **Clients must include this JWT** in the header of their requests to access protected resources on the server.
- The server **validates the JWT** with each request to ensure it's from a legitimate, registered client.

## Database

To maintain the integrity of your database schema, it's essential to create new migrations whenever you make changes to your Entity Framework classes. Whether you're running the application without Docker or with Docker, follow these steps to update your database schema:

1. Make changes to your Entity Framework classes.

2. Open a terminal in the server directory.

3. Run the following commands:
   ```bash
   dotnet ef migrations add <New name for the migrations> --project ../GameManager/GameManager.csproj --startup-project Server.csproj
   ```

## Getting Started without Docker

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
1.  ```bash 
        dotnet ef migrations add <New name for the migrations> --project ../GameManager/GameManager.csproj --startup-project Server.csproj
    ```
2.  ```bash
        dotnet ef database update --project ../GameManager/GameManager.csproj --startup-project Server.csproj
    ```

## Running with Docker

This application can be easily run using Docker. Docker allows you to create and run applications within containers, ensuring that your application runs consistently across different environments.

### Prerequisites

- Docker Desktop (Install from [Docker's official website](https://www.docker.com/products/docker-desktop))

### Building and Running the Docker Container

1. Clone the repository to your local machine if you haven't already.

2. Open a terminal and navigate to the project directory.

3. Build the Docker image using the provided Dockerfile:

   ```bash
   docker build -t connect4game-server .
    ```
Replace connect4game-server with your preferred image name.

4.  Run the Docker container:
    ```bash
        docker run -d -p 8080:80 --name connect4-server connect4game-server
    ```
    This command will start the Docker container named `connect4-server` and map port 8080 on your host to port 80 in the container.

5.  Your Connect4Game Server is now running within a Docker container. You can access it in your web browser at http://localhost:8080.

### Stopping and Removing the Docker Container

To stop and remove the Docker container when you're done:
```bash
    docker stop connect4-server
    docker rm connect4-server

```

## Running with Visual Studio

If you prefer to develop and run the application with Visual Studio, follow these steps:

1.  Open the solution file (Server.sln) in Visual Studio.

2.  Set the startup project to the Server project.

3.  Configure your database connection string in the appsettings.json file of the Server project.

4.  Build and run the application using Visual Studio. The application should launch in your default web browser.

## Running with Kubernetes

This application is also configured to run on Kubernetes, an open-source container orchestration platform. This method is ideal for scaling and deploying the application across a cluster of machines.

### Prerequisites
- [Minikube](https://minikube.sigs.k8s.io/docs/start/) or a Kubernetes cluster setup
- [kubectl](https://kubernetes.io/docs/tasks/tools/) - Kubernetes command-line tool

### Deployment Steps:

1. **Building the Docker Image:**
   
   Before deploying to Kubernetes, ensure you have the Docker image built and available in a container registry (like DockerHub).

    ```bash
        docker build -t [YOUR_DOCKERHUB_USERNAME]/connect4game-server:vX.Y.Z .
    ```
    Replace [YOUR_DOCKERHUB_USERNAME] with your DockerHub username, and vX.Y.Z with the version tag you want to give your Docker image.

    Push the image to DockerHub:
    ```bash
        docker push [YOUR_DOCKERHUB_USERNAME]/connect4game-server:vX.Y.Z
    ```

2.  **Update Kubernetes Configuration:**

    Open your server-deployment.yaml file. Look for the image field under spec.containers. Update the image reference to the one you just pushed to DockerHub.

    ```bash
        spec:
            containers:
            - name: connect4game-server
                image: [YOUR_DOCKERHUB_USERNAME]/connect4game-server:vX.Y.Z
    ```
    Save the changes. You can then apply these changes to your Kubernetes cluster.

3.  **Deploying to Kubernetes:**

    Use kubectl to deploy your services and deployments:
    ```bash
        kubectl apply -f server-deployment.yaml
        kubectl apply -f server-service.yaml
        kubectl apply -f db-deployment.yaml
        kubectl apply -f db-service.yaml
    ```

    Monitor your pods and services:
    ```bash
        kubectl get pods
        kubectl get svc
    ```
4.  **Accessing the Application:**

    If your server service is of type NodePort (as is common with Minikube), you can access it using the Minikube IP and the allocated NodePort:
    ```bash
        minikube ip
    ```
    Combine this IP with the NodePort from the service (e.g., 30080) to access the application, like http://[MINIKUBE_IP]:30080.

### Cleanup:
To remove the deployed resources from Kubernetes: 
```bash
    kubectl delete -f server-deployment.yaml
    kubectl delete -f db-deployment.yaml
```


## Code Practices

This project aims to follow good coding standards and practices. One of the key implementations is the use of partial views to avoid code duplication. 

In the context of MVC architecture, views often share code with other views. To DRY (Don't Repeat Yourself) the code, common code was extracted into partial views. These partial views are then used in multiple other views, enhancing maintainability and promoting code reusability.
