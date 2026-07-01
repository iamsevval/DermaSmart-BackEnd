# DermaSmart Backend API ⚙️

This is the server-side infrastructure of the DermaSmart skincare application. Developed in the .NET environment with the vision of "A skin expert in everyone's pocket," this project hosts a smart rule engine (rule-based engine) that prevents ingredient conflicts. It uses Entity Framework Core and SQLite for database management.

---

## 🚀 Backend (API) Technologies

- **Framework:** ASP.NET Core 8.0 (Web API)
- **Programming Language:** C# (.NET 8)
- **Architectural Approach:** Layered Architecture and MVC Design Pattern (Controllers, Services, Models, Data) 
- **Database:** SQLite
- **ORM:** Entity Framework Core
- **Security & Authentication:** JWT (JSON Web Token), BCrypt, Rate Limiting, CORS
- **API Documentation:** Swagger (OpenAPI)
- **Data Processing (Business Logic):** Custom Skin and Symptom Analysis Services

---

## 🛠️ Installation and Running Steps

Follow the steps below in order in your terminal to run the project on your own computer (locally).

### 1️⃣ Restore Missing Packages

To download the API project's dependencies:

```bash
dotnet restore DermaSmart.API.csproj
```

> When successful, you will see an output similar to `"Restore completed"` in the terminal.

---

### 2️⃣ Update the Database

To create the database schema and apply the migrations:

```bash
dotnet ef database update --project DermaSmart.API.csproj
```

---

### 3️⃣ Run the Project

To start the server:

```bash
dotnet run --project DermaSmart.API.csproj
```

---

## 📖 Swagger API Documentation

Once the project has been run successfully, you can access the Swagger interface to test and inspect the API endpoints.

Use the following addresses in your browser:

### Local Development

```txt
http://localhost:<PORT>/swagger
```

### Production

```txt
http://<to_be_added_soon>/swagger
```

Through the Swagger interface, you can test all HTTP operations directly from the browser, such as:

- GET
- POST
- PUT
- DELETE

## Documentation

The details of the API endpoints, along with the outgoing and incoming JSON data structures (schemas), have been modularly separated in the **`Docs`** folder to facilitate teamwork:

* 🔐 **[Authentication (Auth) API Documentation](./Docs/AUTH_API_DOCUMENTATION.md)**
* 👤 **[Skin Profile API Documentation](./Docs/SKIN_PROFILE_API.md)**

---

## Developer Notes and Improvements (Weekly Scrum Summary)
* **Database and Model Consolidation (Recent Change):** To resolve the table confusion that arose after the merge, the `AppUser` and `AppSkinProfile` tables were completely removed from the system. The project was fully reconnected to the standard `User` and `SkinProfile` models from the ground up. This resolved the 500 errors and database inconsistencies.
* **Ingredient Conflict Rule Engine:** The backend algorithm that detects ingredient conflicts such as niacinamide, glycolic acid, and vitamin C was integrated into the API.
* **JWT Integration:** JWT infrastructure was set up on the backend side for secure session management.
* **Error Codes:** To work in alignment with the mobile team (Developer 4), an `errorCode` key was added to all endpoints that return errors, standardizing the JSON schemas. The mobile side can now perform UI checks based on these error codes (`EMAIL_ALREADY_EXISTS`, `INVALID_CREDENTIALS`, etc.) instead of reading strings.
* **Form Integration:** Skin type records are stored relationally in the updated database and are successfully and fully served via `/api/skinprofile`.

---

## 👥 Development Team (Scrum Team)
This project was developed by a cross-functional Scrum team of 6 people following Agile software principles:
* **Hayrunnida Şahin** (Product Owner)
* **Şevval Arslan** (Scrum Master)
* **Zeynep Ekinci** (Backend Developer)
* **Senanur Turunç** (Backend Developer)
* **Senanur Kurşun** (Frontend Developer)
* **Ayşenur Küçükaslan** (Test / QA)
