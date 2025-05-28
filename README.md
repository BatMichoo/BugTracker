# BugTracker Backend API

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

This repository contains the **Backend API** for the BugTracker application. It's built with ASP.NET Core (C#) and provides the core logic and data management for tracking and managing software bugs and issues. This API is designed to be consumed by a separate frontend application.

---

## ✨ Features

* **RESTful API:** Provides a clean and structured API for interacting with bug data.
* **Data Management:** Handles creation, retrieval, updating, and deletion (CRUD) of bug records.
* **Database Integration:** Connects to a SQL Server database to persist bug information.
* **Unit Testing:** Includes a dedicated project for comprehensive unit tests to ensure API reliability.

---

## 🚀 Technologies Used

* **Backend Framework:** C# (.NET 8.0 or newer)
* **Web Framework:** ASP.NET Core Web API
* **Database:** SQL Server
* **Testing:** nUnit (for unit tests)
* **Build Automation:** dotnet CLI

---

## 📋 Getting Started

These instructions will help you set up and run the BugTracker Backend API on your local machine for development and testing purposes.

### Configure Environment Variables

This project uses environment variables for configuration. For local development, these are typically loaded from the `.env.development` file found in the root of the `BackEnd/` directory.

1.  Create the `.env.development` file in the `BackEnd/` directory.
2.  **Add its contents.** This file should define crucial settings such as:
    * `SQL_SERVER_HOST`: The hostname of your SQL Server instance.
    * `SQL_DATABASE`: The name of the database.
    * `SQL_USER`: The username for database access.
    * `SA_PASSWORD`: The password for the SQL user.
    * `JWT_SECRET_KEY`: The secret key used for JWT token generation/validation.
    * `ASPNETCORE_ENVIRONMENT`: Specifies the ASP.NET Core environment (e.g., `Production`).
    * `HTTP_SERVER_PORT`, `HTTPS_SERVER_PORT`: Ports the backend server will listen on.
    * `REACT_APP_PORT`, `DOMAIN_URL`: Frontend-related variables, useful if this backend is part of a larger monorepo.

    **Example `.env.development` content (as provided):**

    ```properties
    # .env.development

    ASPNETCORE_ENVIRONMENT=Production
    SQL_SERVER_HOST=sqlserver  # IMPORTANT: Change to 'localhost' if running SQL Server directly on your machine
    SQL_DATABASE=BugTracker
    SQL_USER=sa
    SA_PASSWORD=securePassword1!
    JWT_SECRET_KEY=
    HTTP_SERVER_PORT=80
    HTTPS_SERVER_PORT=443
    REACT_APP_PORT=3000
    DOMAIN_URL=localhost
    ```
3.  **Adjust `SQL_SERVER_HOST`:** If you are running SQL Server directly on your machine (not via a Docker service from the main repository's `docker-compose.yml`), **change `SQL_SERVER_HOST=sqlserver` to `SQL_SERVER_HOST=localhost`** in your `.env.development` file.

---
