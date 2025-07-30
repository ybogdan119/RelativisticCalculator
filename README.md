# 🌌 Relativistic Calculator API

An ASP.NET Core Web API that provides relativistic travel time calculations and star data management based on the theory of special relativity.

---

## ✨ Features

- 🚀 Calculate travel time at relativistic speeds:
  - From raw values (distance, acceleration)
  - Using predefined stars (by name)
- 🌟 Manage star data (CRUD)
- ⚙️ High-precision physics constants
- 💥 Conflict detection when inserting duplicate stars
- ✅ Middleware-based global exception handling
- 📄 Swagger (OpenAPI) documentation support

---

## 🛠 Technologies

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQLite**
- **Swagger / Swashbuckle**
- **C# 12**

---

## ⚙️ Configuration

The project uses configurable physical constants:

```json
"PhysicalConstants": {
  "G": 9.80665,
  "C": 299792458,
  "LightYearInMeters": 9.4607304725808e15
}
```

You can modify these in `appsettings.json`.

---

## ▶️ API Overview

### Relativistic Calculations

| Endpoint | Description |
|----------|-------------|
| `POST /calculator/calculate/by-value` | Calculate using raw distance and acceleration |
| `POST /calculator/calculate/by-name` | Calculate using a star's name |

### Star Management

| Endpoint | Description |
|----------|-------------|
| `GET /star/all` | Get all stars |
| `GET /star/{id}` | Get star by ID |
| `GET /star/{name}` | Get star by name |
| `POST /star` | Create new star |
| `POST /star/batch?allowPartialInsert={bool}` | Batch insert (with optional partial success) |
| `PUT /star/{id}` | Update existing star |
| `DELETE /star/{id}` | Delete star |

---

## 🧪 Error Handling

Handled globally via middleware:
- `400` – Invalid input or logic error
- `404` – Resource not found
- `409` – Conflict (e.g., duplicate star)
- `500` – Internal server error

---

## 🚀 Getting Started

1. **Clone the repo**  
   ```bash
   git clone https://github.com/ybogdan119/RelativisticCalculator.git
   ```

2. **Run EF migrations** (optional)  
   ```bash
   dotnet ef database update
   ```

3. **Run the app**  
   ```bash
   dotnet run
   ```

4. **Explore the API**  
   Visit: `https://localhost:{port}/swagger`

---

## 📂 Example JSON Request

**POST** `/calculator/calculate/by-value`

```json
{
  "accelerationG": 1,
  "distanceLightYears": 4.2,
  "decelerateAtTarget": true
}
```

---

## ✅ Sample Star DTO

```json
{
  "id": 1,
  "name": "Proxima Centauri",
  "distanceLy": 4.24
}
```
