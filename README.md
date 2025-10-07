Got you 👍 — the formatting breaks on GitHub because you pasted plain text instead of proper Markdown (missing backticks, table pipes, and spacing).

Here’s the **fully corrected, cleanly formatted GitHub-ready `README.md`**.
Just copy-paste it directly — no editing needed. It will render beautifully on GitHub.

---

# 🧭 Microservices Gateway with Ocelot and Swagger (.NET 9)

This solution demonstrates a simple **API Gateway** built with **Ocelot** that routes traffic to two downstream microservices:

* **AuthService** — handles authentication and issues JWT tokens.
* **DataService** — exposes example data endpoints.

The gateway also aggregates both services’ **Swagger documentation** using **SwaggerForOcelot**, providing a unified API explorer at a single URL.

---

## 🚀 Tech Stack

| Component                  | Purpose                                     |
| -------------------------- | ------------------------------------------- |
| **.NET 9**                 | Framework for all services                  |
| **Ocelot**                 | API Gateway / Reverse Proxy                 |
| **MMLib.SwaggerForOcelot** | Aggregates Swagger from downstream services |
| **Swashbuckle.AspNetCore** | Swagger UI for microservices                |
| **JWT Bearer Auth**        | Token-based authentication (AuthService)    |

---

## 🏗️ Architecture Overview

```
┌─────────────────────────────┐
│        API Gateway          │
│ (Ocelot + SwaggerForOcelot) │
│       Port: 5123            │
└───────┬───────────┬─────────┘
        │           │
        ▼           ▼
┌─────────────────────────────┐    ┌─────────────────────────────┐
│        AuthService          │    │         DataService          │
│  Issues JWT tokens          │    │  Returns product data        │
│  Port: 5138                 │    │  Port: 5146                 │
└─────────────────────────────┘    └─────────────────────────────┘
```

---

## 📂 Project Structure

```
MicroservicesOcelotDemo/
│
├── ApiGateway/
│   ├── Program.cs
│   ├── ocelot.json
│
├── AuthService/
│   ├── Program.cs
│
├── DataService/
│   ├── Program.cs
│
└── TestClient/
    └── Program.cs
```

---

## ⚙️ Setup Instructions

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/MicroservicesOcelotDemo.git
cd MicroservicesOcelotDemo
```

### 2️⃣ Restore Dependencies

```bash
dotnet restore
```

### 3️⃣ Run Each Service in Separate Terminals

```bash
# Auth Service (port 5138)
cd AuthService
dotnet run --urls http://localhost:5138
```

```bash
# Data Service (port 5146)
cd DataService
dotnet run --urls http://localhost:5146
```

```bash
# API Gateway (port 5123)
cd ApiGateway
dotnet run --urls http://localhost:5123
```

---

## 🌐 Access Points

| URL                                                                            | Description                      |
| ------------------------------------------------------------------------------ | -------------------------------- |
| **[http://localhost:5123/swagger](http://localhost:5123/swagger)**             | Aggregated Swagger (Auth + Data) |
| **[http://localhost:5123/auth/token](http://localhost:5123/auth/token)**       | Token issuing endpoint (public)  |
| **[http://localhost:5123/data/products](http://localhost:5123/data/products)** | Data endpoint (example)          |
| **[http://localhost:5138/swagger](http://localhost:5138/swagger)**             | AuthService Swagger              |
| **[http://localhost:5146/swagger](http://localhost:5146/swagger)**             | DataService Swagger              |

---

## 🧭 Gateway Routing Summary

| Gateway Path | Forwards To               | Description         |
| ------------ | ------------------------- | ------------------- |
| `/auth/*`    | `http://localhost:5138/*` | Auth endpoints      |
| `/data/*`    | `http://localhost:5146/*` | Data endpoints      |
| `/swagger`   | Aggregated docs           | SwaggerForOcelot UI |

---

## 🧪 Testing

### ✅ Get a Token

```bash
curl -X POST http://localhost:5123/auth/token \
  -H "Content-Type: application/json" \
  -d '{ "username": "demo", "password": "demo" }'
```

### ✅ Access Protected Data

```bash
curl http://localhost:5123/data/products \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

## 🧩 Key Configuration Files

### ApiGateway/ocelot.json

```json
{
  "Routes": [
    {
      "UpstreamPathTemplate": "/auth/{everything}",
      "UpstreamHttpMethod": [ "Get", "Post", "Put", "Delete", "Options" ],
      "DownstreamPathTemplate": "/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [ { "Host": "localhost", "Port": 5138 } ]
    },
    {
      "UpstreamPathTemplate": "/data/{everything}",
      "UpstreamHttpMethod": [ "Get", "Post", "Put", "Delete", "Options" ],
      "DownstreamPathTemplate": "/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [ { "Host": "localhost", "Port": 5146 } ]
    }
  ],
  "SwaggerEndPoints": [
    {
      "Key": "auth",
      "TransformByOcelotConfig": true,
      "Config": [
        { "Name": "Auth Service", "Version": "v1", "Url": "http://localhost:5138/swagger/v1/swagger.json" }
      ]
    },
    {
      "Key": "data",
      "TransformByOcelotConfig": true,
      "Config": [
        { "Name": "Data Service", "Version": "v1", "Url": "http://localhost:5146/swagger/v1/swagger.json" }
      ]
    }
  ],
  "GlobalConfiguration": { "BaseUrl": "http://localhost:5123" }
}
```

---

## 💡 Notes

* The gateway is configured purely as a **reverse proxy** — no authentication required.
* Both microservices have independent Swagger UIs.
* The gateway’s Swagger aggregates both under `/swagger`.
* You can later enable JWT validation or rate limiting if needed.

---

## 📦 NuGet Packages

| Project                   | Package                                         | Purpose                     |
| ------------------------- | ----------------------------------------------- | --------------------------- |
| ApiGateway                | `Ocelot`                                        | Reverse proxy / API Gateway |
| ApiGateway                | `MMLib.SwaggerForOcelot.AspNetCore`             | Aggregates Swagger docs     |
| AuthService               | `Swashbuckle.AspNetCore`                        | Swagger UI                  |
| DataService               | `Swashbuckle.AspNetCore`                        | Swagger UI                  |
| AuthService / DataService | `Microsoft.AspNetCore.Authentication.JwtBearer` | JWT auth                    |

---

## 📜 License

This project is licensed under the **MIT License**.
Feel free to use, modify, and share.

---

## 👤 Author

**Spyros Ponaris**
💼 [LinkedIn](https://www.linkedin.com/in/spyros-ponaris-913a6937/)
-------------------------------------------------------------------

✅ Copy this text exactly as-is into your `README.md` — it will render perfectly on GitHub with headings, tables, and code blocks.
