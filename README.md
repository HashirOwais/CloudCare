# CloudCare Suite

Welcome to the CloudCare Suite repository. CloudCare is a comprehensive, enterprise-grade solution designed to streamline daycare management. This repository contains the core components of the CloudCare platform, including the backend API and the web frontend, structured as a .NET monorepo.

The live API is deployed in a secure, segmented network within my personal homelab, running on a RHEL VM behind a Traefik reverse proxy.

## About The Project

This project was born out of a real-world need identified while volunteering at a local daycare. The initial prototype, a simple Python script, has been completely re-architected and rewritten into a robust, full-stack application. This evolution reflects my growth as a software engineer, applying industry best practices learned through co-op experiences to build a secure, scalable, and feature-rich platform.

## Architecture Overview

The CloudCare suite is composed of three main components: a web front-end, a back-end API, and a face recognition service. The API and the new Blazor Web App are developed together in this monorepo to streamline development, while the Face Recognition Attendance System remains a separate, standalone service.

```mermaid
graph TD
    subgraph "CloudCare Suite"
        subgraph "This Repository"
            A[CloudCare Web (Blazor)]
            B[CloudCare API (.NET)]
        end
        C(Face Recognition Service)
    end

    subgraph User Interaction
        U[User]
    end

    U -- "Manages Daycare via UI" --> A
    A -- "REST API Calls" --> B
    C -- "REST API Calls (Logs Attendance)" --> B

    style A fill:#6B9AC4,color:#000,stroke:#333,stroke-width:2px
    style B fill:#7FC6A6,color:#000,stroke:#333,stroke-width:2px
    style C fill:#B490B2,color:#000,stroke:#333,stroke-width:2px
```

### Future Direction
The long-term vision for CloudCare is to evolve this architecture towards a more distributed **microservices model**. This will allow for greater scalability, independent deployments, and technological flexibility for each service.

---

## Components

### 1. CloudCare API

The core backend REST API that handles all business logic, data processing, and communication with the database.

*   **Purpose:** Provides a RESTful API for managing users, expenses, and other daycare-related data.
*   **Key Technologies:**
    *   **Framework:** .NET 10 (ASP.NET Core)
    *   **Database:** Entity Framework Core with PostgreSQL
    *   **Authentication:** Auth0 & JWT Bearer Tokens
    *   **Testing:** xUnit
    *   **Observability:** OpenTelemetry

### 2. CloudCare Web

A modern frontend application providing a rich and interactive user experience that runs directly in the browser.

*   **Purpose:** Provides the primary user interface for daycare staff and administrators.
*   **Key Technologies:**
    *   **Framework:** Blazor WebAssembly
    *   **Language:** C#
    *   **Authentication:** OIDC Client
    
### 3. Shared Libraries

To promote code reuse and maintainability, the solution uses several shared class libraries.
*   **`CloudCare.Business`**: Contains core business logic, services, and DTOs.
*   **`CloudCare.Data`**: Responsible for data access, containing the Entity Framework Core DbContext, models, and repository implementations.

### 4. Face Recognition Attendance System (External)

A standalone Python service that provides real-time face recognition for automated attendance tracking.

*   **GitHub Repository:** [https://github.com/HashirOwais/Face_Recognition_Attendance_System.git](https://github.com/HashirOwais/Face_Recognition_Attendance_System.git)
*   **Purpose:** Captures video, recognizes faces, and sends attendance data to the CloudCare API.
*   **Key Technologies:** Python, OpenCV, DeepFace.

---

## CI/CD Pipeline

This project utilizes a CI/CD pipeline powered by GitHub Actions. On every push to `main` or `feature/monorepo-refactor`, the pipeline automatically:
1.  **Restores, builds, and tests** the entire .NET solution to ensure code quality and correctness.
2.  **Builds and pushes** a new Docker image for the `CloudCare.API` to Docker Hub, tagged with `latest` and the commit SHA.
3.  **Triggers a deployment** in the production homelab environment by updating a GitOps repository, which signals Dokploy to pull the new image and redeploy the application.

This automated workflow ensures that every change is validated and deployed reliably.

## Getting Started

To get a local copy of the backend services up and running, follow these steps.

### Prerequisites

-   [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
-   [Docker Desktop](https://www.docker.com/products/docker-desktop)
-   An Auth0 account for authentication.

### Installation & Running

1.  **Clone the repository**
    ```sh
    git clone https://github.com/HashirOwais/CloudCare_API.git
    cd CloudCare_API
    ```
2.  **Set up environment variables**
    Create a `.env` file in the root of the repository. You can copy the `.env.example` file if it exists. Populate it with your secrets:
    ```
    DB_CONN=Server=postgres-db;Database=cloudcare;User Id=postgres;Password=postgres;
    AUTH0_AUTHORITY=<YOUR_AUTH0_AUTHORITY>
    AUTH0_AUDIENCE=<YOUR_AUTH0_AUDIENCE>
    OTEL_ENDPOINT=<YOUR_OTEL_COLLECTOR_ENDPOINT>
    ```
3.  **Run with Docker Compose**
    The easiest way to run the backend API and the PostgreSQL database is with Docker Compose.
    ```sh
    docker-compose up --build
    ```
    The API will be available at `http://localhost:5001`.

4.  **Running the Blazor Frontend**
    To run the frontend, you will need to navigate to its directory in a separate terminal and run it.
    ```sh
    cd src/CloudCare.Web
    dotnet run
    ```

## Contact

Hashir Owais - [hashir15@hotmail.com](mailto:hashir15@hotmail.com)