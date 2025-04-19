# 📚 SimpleBookManager

A web application built using **ASP.NET Core MVC** and **Entity Framework Core** for managing books. It includes features like user authentication, CRUD operations for books, session management, and Razorpay integration for payment handling.

---

## 🎯 Objective

To create a fully functional book management system where users can register/login, manage their personal book collection, and make payments for bookings using Razorpay.

---

## 🛠️ Technologies Used

- ASP.NET Core MVC
- Entity Framework Core (with SQLite)
- Razor Views (.cshtml)
- Razorpay Payment Gateway
- Bootstrap (for styling)
- Session Management

---

## 📁 Project Structure (MVC)

### 📂 Models
Defines the structure of your data:
- `Book.cs`: Properties like Title, Author, Price, etc.
- `User.cs`: Represents registered users.
- `Payment.cs`: Stores Razorpay payment details.

### 📂 Views
Handles the UI (HTML + Razor):
- `Home/Index.cshtml`: Landing page.
- `Books/Index.cshtml`: Book listing.
- `Books/Create.cshtml`: Add a new book.
- `Account/Login.cshtml`, `Register.cshtml`: Authentication views.

### 📂 Controllers
Handles request logic:
- `HomeController.cs`: Loads the homepage.
- `BooksController.cs`: Handles all book operations (Create, Read, Update, Delete).
- `AccountController.cs`: Handles login, registration, logout.

---

## 🔐 Features

- ✅ User Registration and Login
- ✅ Session-based Authentication
- 📖 CRUD operations for books
- 💳 Razorpay integration for book payment
- 📜 SQLite database (created on startup)

---

## 🚀 How to Run

1. **Clone the repo**
   ```bash
   git clone https://github.com/yourusername/SimpleBookManager.git
