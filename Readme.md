# 🛒 UNIFY - E-Commerce Platform (ASP.NET Core MVC)
UNIFY is a full-featured **E-Commerce Platform** built using **ASP.NET Core MVC 8.0** and **SQL Server**. It provides a complete shopping experience for customers and efficient management tools for admins.
---
## 📚 Project Overview
This application is designed using the **MVC architecture** and supports:
- 🛍️ Product Catalog & Inventory
- 🛒 Shopping Cart
- 📦 Order Management
- 💳 Mock Payment Gateway
- 👥 User Management with Role-Based Access
- 🔐 Session-Based Authorization 
## 📁 Core Modules & Branches
| Module              | Git Branch Name      | Status         |
|---------------------|----------------------|----------------|
| Product Management  | `Product`            | 🚧 In Progress |
| Cart & Checkout     | `Cart`               | 🚧 In Progress |
| Order Processing    | `Order`              | 🚧 In Progress |
| Payment Gateway     | `Payment`            | 🚧 In Progress |
| Final QA Testing    | `QA`                 | 🔄 Integration |
| Production Release  | `main`               | 🔐 Stable      |
---
## 👥 Git Collaboration Workflow
We follow a **feature-branch → QA → main** strategy:

### 🔧 Step-by-Step Setup (First-Time Only)
1. **Clone the repository**
  ```bash
  inside a new folder name UNIFY open folder copy the file path and paste in bash
  git clone https://github.com/A123KP/UNIFY2.git
  cd UNIFY
  git branch -a //to check branches
  git checkout QA
  git pull origin QA 
  //do ur changes once done
  git checkout -b "UR BRANCH NAME"
  git add .
  git commit -m ""
  git push origin Ur branch
  git checkout QA
  git merge Ur branch 