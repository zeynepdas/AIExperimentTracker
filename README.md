# AIExperimentTracker API

## Project Description
AIExperimentTracker is a RESTful Web API developed with **.NET 9** to manage artificial intelligence projects and their experiments.
The system allows tracking multiple experiments under a single AI project along with their evaluation metrics.

This project is designed following a **layered architecture** approach demonstrated in the course materials, while also integrating **modern .NET 8/9 practices** such as Minimal APIs.

---

## Technologies Used
- .NET 9
- ASP.NET Core Minimal API
- Swagger / OpenAPI
- Entity Framework Core
- Git & GitHub

---

## Architecture Overview
The project follows a layered architecture:

- **Entities**: Database models
- **DTOs**: Data Transfer Objects for API requests and responses
- **Services**: Business logic layer
- **Data**: Database context and ORM configuration
- **Responses**: Standard API response format
- **Middlewares**: Global exception handling

> Note: While the course examples use controller-based endpoints, this project adopts the **Minimal API approach**, which is recommended in recent .NET versions.

---

## Entities & Relationships
- User
- AIProject
- Experiment
- Metric

Relationships:
- One User can have multiple AIProjects
- One AIProject can have multiple Experiments
- One Experiment can have multiple Metrics

---

## API Response Format
All API responses follow a standard format:

```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": {}
}
