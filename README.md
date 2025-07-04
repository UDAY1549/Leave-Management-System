
````markdown
# 📝 Leave Management System - ASP.NET Core MVC

This is a Leave Management System built with ASP.NET Core MVC, Entity Framework Core, and Identity. It allows Admins, Managers, and Employees to manage leave requests efficiently.

---

## 🚀 Features

- User roles: Admin, Manager, Employee
- Role-based access control
- Submit, approve, or reject leave requests
- Leave history and dashboards per role

---

## ⚙️ Technologies Used

- ASP.NET Core MVC (.NET 6+)
- Entity Framework Core
- Identity (User/Roles)
- Bootstrap 5
- MySQL (configurable)
- Repository Pattern

---

## 🛠️ Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/UDAY1549/Leave-Management-System.git
cd LeaveManagementSystemJSE
````

### 2. Set up Database

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

### 3. Seed Data (Roles + Sample Users)

Seeded on first run in `Program.cs`:

```csharp
await IdentitySeed.InitializeAsync(app.Services);
```

---

## 🔐 Login Credentials (Seeded)

| Role     | Email                                                   | Password     |
| -------- | ------------------------------------------------------- | ------------ |
| Admin    | [admin@company.com](mailto:admin@company.com)           | Manager\@123 |
| Manager  | [colinhorton@amce.com](mailto:colinhorton@amce.com)     | Manager\@123 |
| Manager  | [miltoncoleman@amce.com](mailto:miltoncoleman@amce.com) | Manager\@123 |
| Employee | [ellajefferson@acme.com](mailto:ellajefferson@acme.com) | User\@123    |
| Employee | [earlcraig@acme.com](mailto:earlcraig@acme.com)         | User\@123    |

---

## 🖥️ Running the Application

```bash
dotnet build
dotnet run
```

Navigate to:
📍 `http://localhost:5102`

---

## 🏗️ Architecture Notes

* **Repository Pattern** used for separation of concerns (`ILeaveRequestRepository`)
* **Seed Service** initializes Roles + Users with manager–employee relationships
* **Custom Middleware** for authorization checks per role
* **ViewModels** used for strongly-typed UI data binding

---

## 🧪 Testing (Manual)

* Login as Admin: View all pending manager leave requests.
* Login as Manager: Approve/Reject employees in the same team only.
* Login as Employee: Apply, edit, or delete own pending leaves.

````

---

## 📄 2. Architecture Decisions – `ARCHITECTURE.md`

```markdown
# 🏗️ Architecture Overview

## 1. Layered Design

- **Presentation Layer**: ASP.NET Razor Views, HTML + Bootstrap
- **Controller Layer**: Handles logic, authorization, routing
- **Business Layer**: Services and Repositories for clean logic
- **Data Access Layer**: Entity Framework Core (DbContext)

---

## 2. Patterns Used

### ✅ Repository Pattern

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

### ✅ Seed Pattern

Seeds Identity Roles and Users:

* Uses `UserManager` & `RoleManager`
* Assigns `ManagerId` based on `TeamName`

---

## 3. Role-Based Access

* Uses `[Authorize(Roles = "RoleName")]`
* Role-specific dashboards:

  * `/LeaveRequests/AdminDashboard`
  * `/LeaveRequests/ManagerDashboard`

---

## 4. Identity & Authorization

* ASP.NET Identity is used for:

  * User & Role management
  * Login, logout, register
* Seeded users with hardcoded credentials for testing

---

## 5. Database Schema (Simplified)

* `AspNetUsers` ← extends `ApplicationUser`
* `LeaveRequests`

  * `ApplicationUserId` → FK to user
  * `ManagerId` → FK to manager

````

---

## 👤 3. User Guides – `USER_GUIDE.md`

```markdown
# 👤 User Guide - Leave Management System

---

## 1. Admin

### ✅ Capabilities
- Login using `admin@company.com / Manager@123`
- View pending leave requests **by Managers**
- Monitor system usage

### 📍 Route: `/LeaveRequests/AdminDashboard`

---

## 2. Manager

### ✅ Capabilities
- Login as `colinhorton@amce.com` or `miltoncoleman@amce.com`
- Approve/Reject leave requests from their team only
- Cannot approve their own leave

### 📍 Route: `/LeaveRequests/ManagerDashboard`

---

## 3. Employee

### ✅ Capabilities
- Login as `ellajefferson@acme.com` etc.
- Apply for leave
- Edit/Delete **only pending** leave
- Track leave status

### 📍 Route: `/LeaveRequests`

---

## 🛂 Role Summary

| Role     | Can Apply | Can Approve | View Others | Dashboard Access |
|----------|-----------|-------------|-------------|------------------|
| Admin    | ❌        | ❌          | Managers     | Yes              |
| Manager  | ✅        | ✅ (team)   | Own team     | Yes              |
| Employee | ✅        | ❌          | ❌           | No               |
````
