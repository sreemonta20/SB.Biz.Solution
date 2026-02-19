# Order Management System (OMS)

A modern, responsive web application for managing customers, products, and sales orders. Built using the latest **ASP.NET Core MVC (.NET 10)** framework with a **Code-First** database approach.

![.NET](https://img.shields.io/badge/.NET-10.0-512bd4)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-2026-purple)
![Entity Framework](https://img.shields.io/badge/EF%20Core-10.0-cyan)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-blue)
![Status](https://img.shields.io/badge/Status-Active-success)

## 📋 Table of Contents
- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Prerequisites](#-prerequisites)
- [Configuration](#-configuration)
- [Database Setup (Code-First)](#-database-setup-code-first)
- [Running the Application](#-running-the-application)

## 🚀 Features

*   **Dashboard**: Real-time business metrics and visual data representation. (not finished yet)
*   **Customer Management**: Full CRUD operations with server-side validation. (wihout Delete)
*   **Inventory Control**: Product management with stock tracking.(wihout Delete)
*   **Dynamic Order System**: 
    *   Interactive JavaScript-based order creation.
    *   Live price calculation (Line Total & Grand Total).
    *   **AJAX-powered** Modal views for order details.
    *   No Delete view added.
*   **Architecture**: Clean MVC pattern with Dependency Injection and Asynchronous programming.

## 🛠 Tech Stack

*   **IDE**: Visual Studio 2026
*   **Framework**: .NET 10.0 (ASP.NET Core MVC)
*   **Database**: SQL Server / LocalDB
*   **ORM**: Entity Framework Core 10.0 (Code-First)
*   **Frontend**: HTML5, CSS3, Bootstrap 5, JavaScript (ES6+), jQuery

## ⚙️ Prerequisites

Ensure your development environment is set up with:

1.  **[Visual Studio 2026](https://visualstudio.microsoft.com/)** (Community, Professional, or Enterprise).
    *   *Workload required:* "ASP.NET and web development".
2.  **[.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)** (Usually included with VS 2026).
3.  **SQL Server** (SQL Server 2016/2022/2025 or LocalDB).

## 🔧 Configuration

1.  **Clone the repository**
    ```bash
    git clone [https://github.com/your-username/your-repo-name.git](https://github.com/sreemonta20/SB.Biz.Solution.git)
    cd your-repo-name
    ```

2.  **Update Connection String**
    Open `appsettings.json` and configure your SQL Server connection.
    
    *Example for LocalDB:*
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=OrderMgtDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
    }
    ```

    *Example for SQL Server (Named Instance):*
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR-PC-NAME\\SQLEXPRESS;Database=OrderMgtDB;Trusted_Connection=True;TrustServerCertificate=True"
    }
    ```

## 🗄 Database Setup (Code-First)

This project uses **Entity Framework Core 10** to generate the database schema from the C# models.

### Method 1: Visual Studio 2026 (Package Manager Console)
1.  Open the project in Visual Studio 2026.
2.  Navigate to **Tools** > **NuGet Package Manager** > **Package Manager Console**.
3.  Run the following command to generate the migration snapshot:
    ```powershell
    Add-Migration InitialCreate
    ```
4.  Run the following command to create the database and tables in SQL Server:
    ```powershell
    Update-Database
    ```

### Method 2: .NET CLI (Terminal)
If you prefer the command line or VS Code:
1.  Open a terminal in the project root.
2.  Create the migration:
    ```bash
    dotnet ef migrations add InitialCreate
    ```
3.  Apply changes to the database:
    ```bash
    dotnet ef database update
    ```

> **Note:** If you make changes to your Models (e.g., `Customer.cs` or `Order.cs`) in the future, repeat these steps with a new migration name (e.g., `Add-Migration AddProductImage`).

## ▶️ Running the Application

1.  Open the solution file (`.sln`) in **Visual Studio 2026**.
2.  Press **F5** or click the **Green Play Button** (Debug).
3.  The browser will launch automatically at `https://localhost:xxxx`.

---

**Developed by [Your Name]**  
*Built with .NET 10 & VS 2026*
