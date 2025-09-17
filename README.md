# E-Commerce Order Management System (Demo)

![.NET](https://img.shields.io/badge/.NET-9-blueviolet)
![Docker](https://img.shields.io/badge/Docker-Compose-blue)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-Messaging-orange)
![CQRS](https://img.shields.io/badge/Architecture-CQRS-red)
![SQLite](https://img.shields.io/badge/Database-SQLite-blue)

Dit project is een demonstratie van een modern, ontkoppeld backend-systeem voor het beheren van e-commerce bestellingen. Het is gebouwd met .NET 9 en maakt gebruik van geavanceerde architecturele patronen zoals **CQRS (Command Query Responsibility Segregation)** en **Event-Driven Architecture** om een schaalbare, onderhoudbare en veerkrachtige oplossing te creëren.

De gehele applicatie-stack, inclusief de message broker en de services, wordt georkestreerd met **Docker Compose**.

## Architectuur Overzicht

Het systeem is opgesplitst in een **schrijfmodel** (voor het verwerken van commands) en een **leesmodel** (voor het snel ophalen van data). Deze modellen worden gesynchroniseerd via asynchrone berichten (events).

#### De Flow voor het Aanmaken van een Order (Command):
De `API` service is alleen verantwoordelijk voor het accepteren van het command en het publiceren ervan op de message bus. De `Worker` service verwerkt het command, past het schrijfmodel aan, en publiceert een event. Een andere consumer in de `Worker` reageert op dit event om het leesmodel bij te werken.

Client (Postman)
|
V
[ API Service ] --(1. Publish CreateOrderCommand)--> [ RabbitMQ ] --(2. Consume Command)--> [ Worker Service ]
|
V
(3. CommandHandler: Update Write DB)
|
V
(4. Publish OrderCreatedEvent)
|
V
[ RabbitMQ ]
|
V
(5. EventConsumer: Update Read DB)

#### De Flow voor het Ophalen van Data (Query):
Het ophalen van data is een simpele, synchrone operatie. De `API` service bevraagt direct het geoptimaliseerde leesmodel en stuurt de data terug.

Client (Postman) --> [ API Service ] --(MediatR Query)--> [ QueryHandler ] --> [ Read DB (OrderSummaries) ] --> Client

## Technologieën en Libraries

* **Framework:** .NET 9
* **Services:** ASP.NET Core Web API, .NET Worker Service
* **Architectuur:** CQRS, Event-Driven, Domain-Driven Design (DDD) principes
* **Database:** SQLite (via Entity Framework Core)
* **Messaging:**
    * **RabbitMQ:** Robuuste, open-source message broker.
    * **MassTransit:** Hoog-niveau .NET library voor abstractie over RabbitMQ.
* **In-Process Messaging:** MediatR voor het implementeren van het CQRS-patroon binnen de services.
* **Containerisatie:** Docker & Docker Compose

## Projectstructuur

De solution is opgedeeld volgens de principes van Clean Architecture:

-   `src/ECommerce.Orders.Api`: Het ASP.NET Core project dat de `GET` (Query) en `POST/PUT/DELETE` (Command) endpoints blootstelt.
-   `src/ECommerce.Orders.Worker`: De .NET achtergrondservice die luistert naar berichten van RabbitMQ en de daadwerkelijke business-logica uitvoert.
-   `src/ECommerce.Orders.Application`: Bevat de business-logica (Command & Query Handlers).
-   `src/ECommerce.Orders.Domain`: Bevat de kern domein entiteiten (`Order`, `Product`) en business-regels.
-   `src/ECommerce.Orders.Infrastructure`: Bevat de implementaties van externe services, zoals de `DbContext` en Repositories.
-   `src/ECommerce.Orders.Contracts`: Een gedeelde bibliotheek met de definities van de Commands en Events die over de message bus worden verstuurd.

## Getting Started

### Vereisten

-   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
-   [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Applicatie Draaien

Het project is volledig gecontaineriseerd. De makkelijkste manier om alles te starten is via Docker Compose.

1.  Clone deze repository.
2.  Open een terminal in de root van de solution (waar `docker-compose.yml` staat).
3.  Voer het volgende commando uit:

    ```bash
    docker-compose up --build
    ```

Dit commando zal:
-   De Docker images voor de `Api` en `Worker` services bouwen.
-   Een container voor `RabbitMQ` downloaden en starten.
-   Alle drie de containers starten en ze in een gedeeld netwerk plaatsen.

### Toegang tot de Services

-   **API Base URL voor Postman:** [http://localhost:8080]
-   **RabbitMQ Management UI:** [http://localhost:15672](http://localhost:15672) (login: `guest` / `guest`)

## Voorbeeld Workflow

1.  **Start de applicatie** met `docker-compose up --build`.
2.  **Maak een product aan:** Gebruik je API-client (Postman of Swagger) om een `POST` request te sturen naar `http://localhost:8080/api/products`.
    **Body:**
    ```json
    {
      "name": "Draadloze Muis",
      "price": 75.50
    }
    ```
3.  **Maak een order aan:** Gebruik het `POST /api/orders` endpoint. Plak de gekopieerde product-ID in de request body.
    ```json
    {
      "customerId": "klant-123",
      "orderDate": "2025-09-17T14:30:00",
      "totalPrice": 75.50,
      "orderLines": [
        {
          "productId": "DE-GEKOPIEERDE-PRODUCT-ID",
          "quantity": 1,
          "unitPrice": 30
        }
      ]
    }
    ```
4.  **Bekijk de order summary:** Roep `GET /api/orders` aan. Je zult de samenvatting van de order zien die je net hebt aangemaakt (dit kan een fractie van een seconde duren door de "eventual consistency").
5.  **Verwijder de order:** Roep `DELETE /api/orders/{id}` aan met de order-ID die je in de vorige stap hebt gekregen. Je krijgt een `202 Accepted` terug.
6.  **Verifieer de verwijdering:** Roep `GET /api/orders` opnieuw aan. De lijst zal nu leeg zijn, omdat de `OrderSummary` is verwijderd als reactie op het `OrderDeletedEvent`.

## Belangrijkste Concepten Gedemonstreerd

-   **CQRS:** Scheiding van schrijf- (Commands) en leesoperaties (Queries) voor betere performance en schaalbaarheid.
-   **Event-Driven Architectuur:** Services communiceren via asynchrone events, wat leidt tot een zeer ontkoppeld en veerkrachtig systeem.
-   **Message Queuing:** Gebruik van RabbitMQ om de betrouwbaarheid van de communicatie te garanderen. Berichten gaan niet verloren als een service tijdelijk down is.
-   **Eventual Consistency:** Het leesmodel wordt asynchroon bijgewerkt en is "uiteindelijk" consistent met het schrijfmodel.
-   **Containerisatie:** De volledige ontwikkel- en productieomgeving is gedefinieerd in code met Docker.
-   **Clean Architecture:** Een duidelijke scheiding van verantwoordelijkheden tussen de verschillende lagen van de applicatie.
