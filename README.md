# ASP.NET WEB API

APIS Included:
 - REST API
 - Authentication
 - Role Based Access Control

## API Endpoints

### Auth

| Endpoint                    | Functionality        |
| --------------------------- | -------------------- |
| GET `/api/auth/services`        | Get User Details from jwt     (Protected) |
| GET `/api/auth/`                | Get User Details from jwt         (Protected) |
| POST `/api/auth/register`       | Register a user         |
| POST `/api/auth/login`          | Login a user         |

### SuperHeros

| Endpoint                    | Functionality        |
| --------------------------- | -------------------- |
| GET `/api/SuperHeros`         | Get All Superheros from db      |
| GET `/api/SuperHeros/{Id}`         | Get Details of single Superhero from db      |
| POST `/api/SuperHeros`          | Create a new superhero         |
| PUT `/api/SuperHeros/{Id}`          | Update a superhero         |
| DELETE `/api/SuperHeros/{Id}`          | Delete a Superhero         |

### Role base Authentication

| Endpoint                    | Functionality        |
| --------------------------- | -------------------- |
| GET `/WeatherForecast`         | Get weather of some particular cities only asscees by admins {Protected}      |
