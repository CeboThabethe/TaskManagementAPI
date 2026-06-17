# Task Management API

A RESTful API for managing tasks with JWT authentication built with ASP.NET Core Web API.

## Features

- User registration and login
- JWT token-based authentication
- Create, read, update, and delete tasks
- Task status tracking (To Do, In Progress, Done)
- Task priority levels (Low, Medium, High)
- Each user can only access their own tasks
- Swagger API documentation

## Technologies

- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- SQL Server LocalDB
- JWT Authentication
- Swagger/OpenAPI

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server LocalDB or SQL Server

### Run the Application

1. Clone the repository
2. Navigate to the project folder
3. Update the database
4. Run the application
5. Open Swagger



## API Endpoints

### Auth
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/Auth/register | Register a new user |
| POST | /api/Auth/login | Login and get JWT token |

### Tasks (Requires JWT)
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/Tasks | Get all tasks for current user |
| GET | /api/Tasks/{id} | Get a specific task |
| POST | /api/Tasks | Create a new task |
| PUT | /api/Tasks/{id} | Update a task |
| DELETE | /api/Tasks/{id} | Delete a task |

## Example Usage

### 1. Register a User

POST https://localhost:7224/api/Auth/register
Content-Type: application/json

{
"username": "john",
"email": "john@test.com",
"password": "password123"
}

### 2. Login

POST https://localhost:7224/api/Auth/login
Content-Type: application/json

{
  "username": "john",
  "password": "password123"
}

Response:

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "john",
    "email": "john@test.com"
  }
}

### 3. Create a Task (with JWT)

POST https://localhost:7224/api/Tasks
Authorization: Bearer YOUR_TOKEN
Content-Type: application/json

{
  "title": "Build Task API",
  "description": "Create a RESTful API for task management",
  "status": 0,
  "priority": 1,
  "dueDate": "2026-07-01T00:00:00"
}



### Future Improvements

- Password hashing (BCrypt)
- Email verification
- Task sharing between users
- Due date reminders
- Pagination and filtering

### Author
Cebo Thabethe

GitHub Repository
https://github.com/CeboThabethe/TaskManagementAPI.git