# 📋 TaskManager API

A clean, secure REST API for personal task management built with .NET 8,
featuring JWT authentication, PostgreSQL, and full Docker support.

## 🚀 Features

- **JWT Authentication** - Secure register/login with token-based auth
- **Password Security** - BCrypt hashing, never stored in plain text
- **User-Scoped Tasks** - Each user can only access their own tasks
- **Full CRUD** - Create, read, update, delete tasks
- **Auto Migrations** - Database schema applies automatically on startup
- **Swagger UI** - Interactive API documentation with JWT support
- **Dockerized** - Multi-stage build, docker-compose with healthchecks

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | .NET 8.0 Web API |
| Database | PostgreSQL 16 |
| ORM | Entity Framework Core |
| Auth | JWT Bearer Tokens |
| Hashing | BCrypt.Net |
| Docs | Swashbuckle / Swagger |
| Containers | Docker + Docker Compose |

## 📁 Project Structure
TaskManager.API/

├── Controllers/     # API endpoints

├── Services/        # Business logic

├── Models/          # Entities & DTOs

├── Data/            # EF Core DbContext

├── Migrations/      # Database schema history

├── Dockerfile       # Multi-stage build

└── docker-compose.yml

## ⚡ Quick Start (Docker)

```bash
# 1. Clone the repo
git clone https://github.com/Venomreings61/TaskManager.API.git
cd TaskManager.API

# 2. Set up environment variables
cp .env.example .env
# Edit .env with your own values

# 3. Run everything!
docker-compose up --build -d

# 4. Open Swagger
http://localhost:5001/swagger
```

## 🔑 API Endpoints

| Method | Endpoint | Auth Required | Description |
|---|---|---|---|
| POST | `/api/auth/register` | No | Create new account |
| POST | `/api/auth/login` | No | Login and get JWT token |
| GET | `/api/task` | Yes | Get all your tasks |
| GET | `/api/task/{id}` | Yes | Get a specific task |
| POST | `/api/task` | Yes | Create a new task |
| PUT | `/api/task/{id}` | Yes | Update a task |
| DELETE | `/api/task/{id}` | Yes | Delete a task |

## 🔐 Authentication Flow

1. Register: `POST /api/auth/register`
2. Login: `POST /api/auth/login` → receive JWT token
3. In Swagger, click **Authorize** and enter: `Bearer <your-token>`
4. Now access protected `/api/task` endpoints!

## 🐳 Docker Architecture
┌─────────────────────────────────────┐

│       taskmanager-network            │

│                                       │

│  ┌──────────────┐  ┌──────────────┐  │

│  │ taskmanager- │  │ taskmanager- │  │

│  │ api (8080)   │──│ db (5432)    │  │

│  └──────────────┘  └──────────────┘  │

│         ↑                            │

└─────────┼─────────────────────────────┘

│

localhost:5001

# 📋 TaskManager API

![Docker Pulls](https://img.shields.io/docker/pulls/megalodon61/taskmanager-api)
![Docker Image Size](https://img.shields.io/docker/image-size/megalodon61/taskmanager-api/latest)
[![Docker Hub](https://img.shields.io/badge/Docker%20Hub-megalodon61%2Ftaskmanager--api-blue?logo=docker)](https://hub.docker.com/r/megalodon61/taskmanager-api)


## 🐳 Run Directly from Docker Hub (No Clone Needed!)

```bash
docker pull megalodon61/taskmanager-api:v1
docker run -d -p 5000:8080 \
  -e ConnectionStrings__DefaultConnection="Host=your-db-host;Database=TaskManagerDb;Username=postgres;Password=yourpassword" \
  -e JWT__Secret="your-secret-key" \
  megalodon61/taskmanager-api:v1
```

## 🧪 Run Locally (without Docker)

```bash
dotnet restore
dotnet ef database update
dotnet run
```

## 👨‍💻 Author

**Santo** | Backend Developer
- GitHub: [@Venomreings61](https://github.com/Venomreings61)
- Built as part of a 90-day Linux/Git/Docker/AWS learning journey
