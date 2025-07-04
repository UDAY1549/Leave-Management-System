
````markdown
 📝 Leave Management System - ASP.NET Core MVC

This is a Leave Management System built with ASP.NET Core MVC, Entity Framework Core, and Identity. It allows Admins, Managers, and Employees to manage leave requests efficiently.

---

 🚀 Features

- User roles: Admin, Manager, Employee
- Role-based access control
- Submit, approve, or reject leave requests
- Leave history and dashboards per role

---

 ⚙️ Technologies Used

- ASP.NET Core MVC (.NET 6+)
- Entity Framework Core
- Identity (User/Roles)
- Bootstrap 5
- MySQL (configurable)
- Repository Pattern

---

 🛠️ Getting Started

 1. Clone the Repository

```bash
git clone https://github.com/UDAY1549/Leave-Management-System.git
cd LeaveManagementSystemJSE
````

 2. Set up Database

Update `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Your_Connection_String_Here"
}
```

Then apply migrations:

```bash
dotnet ef database update
```

 3. Seed Data (Roles + Sample Users)

Seeded on first run in `Program.cs`:

```csharp
await IdentitySeed.InitializeAsync(app.Services);
```

---

 🔐 Login Credentials (Seeded)

| Role     | Email                                                   | Password     |
| -------- | ------------------------------------------------------- | ------------ |
| Admin    | [admin@company.com](mailto:admin@company.com)           | Manager\@123 |
| Manager  | [colinhorton@amce.com](mailto:colinhorton@amce.com)     | Manager\@123 |
| Manager  | [miltoncoleman@amce.com](mailto:miltoncoleman@amce.com) | Manager\@123 |
| Employee | [ellajefferson@acme.com](mailto:ellajefferson@acme.com) | User\@123    |
| Employee | [earlcraig@acme.com](mailto:earlcraig@acme.com)         | User\@123    |

---

 🖥️ Running the Application

```bash
dotnet build
dotnet run
```

Navigate to:
📍 `http://localhost:5102`

---

 🏗️ Architecture Notes

* **Repository Pattern** used for separation of concerns (`ILeaveRequestRepository`)
* **Seed Service** initializes Roles + Users with manager–employee relationships
* **Custom Middleware** for authorization checks per role
* **ViewModels** used for strongly-typed UI data binding

---

 🧪 Testing (Manual)

* Login as Admin: View all pending manager leave requests.
* Login as Manager: Approve/Reject employees in the same team only.
* Login as Employee: Apply, edit, or delete own pending leaves.

````

---

 📄 2. Architecture Decisions – `ARCHITECTURE.md`

```markdown
 🏗️ Architecture Overview

 1. Layered Design

- **Presentation Layer**: ASP.NET Razor Views, HTML + Bootstrap
- **Controller Layer**: Handles logic, authorization, routing
- **Business Layer**: Services and Repositories for clean logic
- **Data Access Layer**: Entity Framework Core (DbContext)

---

 2. Patterns Used

 ✅ Repository Pattern

Interface-based access to data models:
```csharp
public interface ILeaveRequestRepository {
    Task<IEnumerable<LeaveRequest>> GetByUserIdAsync(string userId);
    Task<LeaveRequest?> GetByIdAsync(int id);
    Task AddAsync(LeaveRequest leaveRequest);
    void Update(LeaveRequest leaveRequest);
    void Delete(LeaveRequest leaveRequest);
    Task SaveAsync();
}
````

 ✅ Seed Pattern

Seeds Identity Roles and Users:

* Uses `UserManager` & `RoleManager`
* Assigns `ManagerId` based on `TeamName`

---

 3. Role-Based Access

* Uses `[Authorize(Roles = "RoleName")]`
* Role-specific dashboards:

  * `/LeaveRequests/AdminDashboard`
  * `/LeaveRequests/ManagerDashboard`

---

 4. Identity & Authorization

* ASP.NET Identity is used for:

  * User & Role management
  * Login, logout, register
* Seeded users with hardcoded credentials for testing

---

 5. Database Schema (Simplified)

* `AspNetUsers` ← extends `ApplicationUser`
* `LeaveRequests`

  * `ApplicationUserId` → FK to user
  * `ManagerId` → FK to manager

````

---

 👤 3. User Guides – `USER_GUIDE.md`

```markdown
 👤 User Guide - Leave Management System

---

 1. Admin

 ✅ Capabilities
- Login using `admin@company.com / Manager@123`
- View pending leave requests **by Managers**
- Monitor system usage

 📍 Route: `/LeaveRequests/AdminDashboard`

---

 2. Manager

 ✅ Capabilities
- Login as `colinhorton@amce.com` or `miltoncoleman@amce.com`
- Approve/Reject leave requests from their team only
- Cannot approve their own leave

 📍 Route: `/LeaveRequests/ManagerDashboard`

---

 3. Employee

 ✅ Capabilities
- Login as `ellajefferson@acme.com` etc.
- Apply for leave
- Edit/Delete **only pending** leave
- Track leave status

 📍 Route: `/LeaveRequests`

---

 🛂 Role Summary

| Role     | Can Apply | Can Approve | View Others | Dashboard Access |
|----------|-----------|-------------|-------------|------------------|
| Admin    | ❌        | ❌          | Managers     | Yes              |
| Manager  | ✅        | ✅ (team)   | Own team     | Yes              |
| Employee | ✅        | ❌          | ❌           | No               |
````

![Screenshot (158)](https://github.com/user-attachments/assets/6a552f0d-4c10-42f4-bf19-8ce37c579e70)
![Screenshot (159)](https://github.com/user-attachments/assets/d934187d-3665-4f5b-a415-86035116fe98)
![Screenshot (160)](https://github.com/user-attachments/assets/f265c6f5-dc6a-4417-b852-457b78fe7cc9)
![Screenshot (161)](https://github.com/user-attachments/assets/1e443999-2e02-4e7d-a199-3a859d17324d)
![Screenshot (162)](https://github.com/user-attachments/assets/b23eb494-e1d1-4e0b-b2ee-b8674ad29f35)
![Screenshot (163)](https://github.com/user-attachments/assets/52908185-f7e4-4ec5-976d-6a874e02f867)
![Screenshot (164)](https://github.com/user-attachments/assets/ce5048ad-2165-4e11-b40f-0a22f6b13315)
![Screenshot (165)](https://github.com/user-attachments/assets/3d11eca5-d901-43e9-b2df-9b603d7e0096)
![Screenshot (166)](https://github.com/user-attachments/assets/c4928bf3-c9df-45c5-90c9-1a9a07aab1aa)





