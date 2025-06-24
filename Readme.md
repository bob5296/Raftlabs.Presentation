# Raftlabs User API

A sample .NET-based API project to demonstrate clean architecture practices, caching strategies, and secure access.

---

## 🚀 Run the Project

Make sure to set the correct startup project before running:

```sh
Set "Raftlabs.Presentation" as the startup project
Press F5 or run from terminal
```

---

## 📌 TODO / Recommendations

* [ ] Add `Directory.Packages.props` for centralized NuGet package version management
* [ ] Use **CQRS** or `IMediator` to decouple **Presentation** from **Application** logic
* [ ] Integrate **Azure Key Vault** for secure configuration and secrets management
* [ ] Use `global using` directives for simplified usings across projects
* [ ] Implement **cache locking**

  * Use `SemaphoreSlim` for in-memory cache locking
  * Use **Redis distributed lock** for multi-node scenarios
* [ ] Large object caching should leverage in-memory cache (if data is expensive to recompute)

---

## 🧪 Test Endpoints Using cURL

### 🔍 Get User by ID

```bash
curl --location 'https://localhost:44397/users/13' \
--header 'accept: application/json' \
--header 'x-api-key: reqres-free-v1'
```

### 📄 Get All Users

```bash
curl --location 'https://localhost:44397/users?pagenumber=1' \
--header 'accept: application/json' \
--header 'x-api-key: reqres-free-v1'
```

> ✅ Tip: You can simplify headers when testing via Postman or Swagger. Only `accept` and `x-api-key` are usually required.

---

## 🔐 Security Notes

* All endpoints are secured using an `x-api-key` header.
* Consider implementing OAuth2 or JWT for advanced scenarios.

---

## 📦 Technologies Used

* .NET Core 8
* Clean Architecture principles (CQRS, DI, layering)
* Redis (optional)
* In-Memory Cache
* Swagger / OpenAPI

---

## 🛠 Suggestions for Production

* Add health check endpoints (`/health`) for load balancer readiness
* Use middleware to handle logging
* Include observability with Application Insights or Serilog
* Enable HTTPS redirection and HSTS headers
