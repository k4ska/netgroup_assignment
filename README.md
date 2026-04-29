# NetGroup Assignment

Simple events app with a Nuxt 4 frontend and an ASP.NET Core backend.

## Prerequisites

- Docker and Docker Compose

## Run locally

1. From the repository root, start all services:

   - `docker compose up --build`

2. Open the app:

   - Frontend: http://localhost:3000

3. Admin login credentials:

   - Use the values in `backend/appsettings.json`:
     - `Admin:Email`
     - `Admin:Password`


## Stop services

- `docker compose down`

## Notes

- The backend uses PostgreSQL and applies EF Core migrations on startup.
- If you change backend settings, rebuild/restart the backend container:
  - `docker compose up --build backend`
