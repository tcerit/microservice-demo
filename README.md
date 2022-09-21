# microservice-demo
A microservice architecture sample adopting DDD(Domain Driven Design) approach with .NET 6.0.
There are three microservices Orders, Customers and Products using PostgreSQL as RDMS, RabbitMQ as MessageBroker, transactional outbox pattern for reliable messaging and docker for containerization.

## What Can You Find in This Sample
* .Net Web API application with REST API principles
* Entity Framework Core 6.0 and Repository Pattern Implementation
* MediatR for Events and CQRS
* Swagger Open API
* Bounded Contexts
* Ardalis.GuardClauses
* RabbitMQ implementation for broadcasting to multiple clients and listening from multiple exchanges
* Transactional Outbox Pattern
* Docker Containers for microservices and infrastructures, docker compose for setting up and deploying all containers.

## Microservice Architecture 

![UML-System-Arch](https://user-images.githubusercontent.com/22146984/191615622-73822e15-d21a-405a-9ede-26c2ec2c0de7.jpg)

## How to Run the Project

### Requirements

* .Net Core 6 or later [Download](https://dotnet.microsoft.com/download/dotnet/6.0)
* Docker Desktop [Download](https://www.docker.com/products/docker-desktop)

### Installing

```
docker compose -f docker-compose.yml up -d
```

#### Notes

* Remove `-d` option to see the console output for the containers

* Check [Compose V2](https://www.docker.com/blog/announcing-compose-v2-general-availability/) just in case you're still using `docker-compose` command.

<img width="961" alt="image" src="https://user-images.githubusercontent.com/22146984/191617512-42ee1967-dd26-4f2b-8a77-e2b774a55a29.png">

 
