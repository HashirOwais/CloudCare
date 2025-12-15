# Gemini Workspace Analysis: /Users/hashir/Dev/CLOUDCARE/CloudCare

This document provides a high-level overview of the projects, technologies, and development patterns found in this workspace. It is intended to serve as a quick-start guide for understanding the user's primary activities.

## 1. Project Overview

The `/Users/hashir/Dev/CLOUDCARE/CloudCare` directory contains the "CloudCare Suite," a comprehensive, microservice-based SaaS application for daycare management. The suite is structured as a .NET monorepo, containing the backend API and the web frontend. An external Python-based face recognition system for attendance is also part of the suite but resides in a separate repository.

## 2. Key Projects

### 2.1. CloudCare Suite

A sophisticated, containerized SaaS platform for daycare management.

*   **Repositories:**
    *   This monorepo contains `CloudCare.API` and `CloudCare.Web`.
    *   An external repository holds the `Face_Recognition_Attendance_System`.
*   **Architecture & Technologies:**
    *   **Frontend (CloudCare.Web):** A Blazor WebAssembly application providing the user interface.
    *   **Backend (CloudCare.API):** A .NET 10 (ASP.NET Core) API using Entity Framework Core with a PostgreSQL database. It handles business logic and data processing. Auth0 and JWT are used for authentication.
    *   **AI Service (External):** A Python service using OpenCV and DeepFace for facial recognition to log attendance.
*   **Deployment & DevOps:**
    *   The frontend is deployed to **Azure Static Web Apps**.
    *   The backend is self-hosted in a **Homelab** on a RHEL VM, managed with **Docker Compose** and fronted by a **Traefik** reverse proxy behind an **OPNsense** firewall.
    *   A full **CI/CD pipeline** using **GitHub Actions** automates building, testing, and deploying the API.

## 3. Building and Running

### Local Development

The backend services can be run locally using Docker Compose.

1.  **Set up environment variables:**
    Create a `.env` file in the root of the repository with the following content:
    ```
    DB_CONN=Server=postgres-db;Database=cloudcare;User Id=postgres;Password=postgres;
    AUTH0_AUTHORITY=<YOUR_AUTH0_AUTHORITY>
    AUTH0_AUDIENCE=<YOUR_AUTH0_AUDIENCE>
    OTEL_ENDPOINT=<YOUR_OTEL_COLLECTOR_ENDPOINT>
    ```

2.  **Run with Docker Compose:**
    ```sh
    docker-compose up --build
    ```
    The API will be available at `http://localhost:5001`.

3.  **Running the Blazor Frontend:**
    ```sh
    cd src/CloudCare.Web
    dotnet run
    ```

### Testing

The project uses **xUnit** for unit testing. Tests can be run from the solution level or project level using the `dotnet test` command.

## 4. Development Conventions

*   **Monorepo:** The API and Web projects are managed in a single repository to streamline development.
*   **Shared Libraries:** Code is shared between projects using `CloudCare.Business` (for business logic) and `CloudCare.Data` (for data access).
*   **Containerization:** Docker is used for local development and production deployment of the backend.
*   **Infrastructure as Code:** The homelab setup and Docker Compose files suggest a commitment to IaC principles.
*   **CI/CD:** Automation is a key part of the workflow, with GitHub Actions handling the build and deployment pipeline.
