# Cosmetics Store Application

## Overview

Cosmetics Store is a desktop application developed with WPF (.NET 8.0) that allows users to browse and purchase cosmetic products. The application follows a three-layer architecture (Repository, Service, and View) and includes a comprehensive user management system.

## Features

- **User Authentication**
  - Login/Logout functionality using email and password
  - User registration
  - Role-based access control (Customer and Manager roles)

- **Customer Features**
  - Browse products
  - View products by category
  - Add products to cart
  - Manage shopping cart
  - Place and view orders

- **Admin Features**
  - Dashboard with statistics
  - Product management
  - Category management
  - Order management
  - User management

## Technical Details

- **Framework**: .NET 8.0, WPF
- **Database**: SQL Server
- **Architecture**: 3-layer architecture
  - Repository Layer - Data access and manipulation
  - Service Layer - Business logic
  - View Layer - User interface

## Getting Started

### Prerequisites

- Visual Studio 2022 or later
- .NET 8.0 SDK
- SQL Server

### Setup

1. Clone the repository
2. Open the solution in Visual Studio
3. Ensure the connection string in `appsettings.json` is configured for your SQL Server instance
4. Run the SQL script in the `Database.sql` file to create the database and populate sample data
5. Build and run the application

### Demo Accounts

- **Admin**: Email: <admin@gmail.com>, Password: 123
- **Customer**: Email: <customer@gmail.com>, Password: 123

## Project Structure

- **CosmeticsStore.Repositories**: Contains database models, context, and repository implementations
- **CosmeticsStore.Services**: Contains business logic and service implementations
- **CosmeticsStore.WPF**: Contains the WPF UI components

## Database Schema

The application uses the following tables:

- Users
- Categories
- Products
- Carts
- CartItems
- Orders
- OrderDetails

## Screenshots

(Screenshots would be added here)

## Future Enhancements

- Product search functionality
- Payment gateway integration
- Inventory management
- Customer reviews and ratings
- Discount and promotion systems
