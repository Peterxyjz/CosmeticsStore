-- Create Cosmetics Database
CREATE DATABASE CosmeticsDB;
GO

USE CosmeticsDB;
GO

-- Create Users Table
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Role NVARCHAR(20) NOT NULL, -- 'Customer' or 'Manager'
    CreatedDate DATETIME DEFAULT GETDATE(),
    Status BIT DEFAULT 1 -- 1 for active, 0 for inactive
);
GO

-- Create Categories Table
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255)
);
GO

-- Create Products Table
CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    CategoryId INT FOREIGN KEY REFERENCES Categories(CategoryId),
    Description NVARCHAR(MAX),
    Price DECIMAL(10, 2) NOT NULL,
    StockQuantity INT NOT NULL,
    ImageUrl NVARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE(),
    Status BIT DEFAULT 1 -- 1 for active, 0 for inactive
);
GO

-- Create Carts Table
CREATE TABLE Carts (
    CartId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    CreatedDate DATETIME DEFAULT GETDATE(),
    Status BIT DEFAULT 1
);
GO

-- Create CartItems Table
CREATE TABLE CartItems (
    CartItemId INT PRIMARY KEY IDENTITY(1,1),
    CartId INT FOREIGN KEY REFERENCES Carts(CartId),
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId),
    Quantity INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL
);
GO

-- Create Orders Table
CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(10, 2) NOT NULL,
    ShippingAddress NVARCHAR(255),
    Status NVARCHAR(50) DEFAULT 'Pending', -- Pending, Processing, Shipped, Delivered, Cancelled
    PaymentMethod NVARCHAR(50),
    PaymentStatus NVARCHAR(50) DEFAULT 'Unpaid' -- Unpaid, Paid
);
GO

-- Create OrderDetails Table
CREATE TABLE OrderDetails (
    OrderDetailId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT FOREIGN KEY REFERENCES Orders(OrderId),
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId),
    Quantity INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL
);
GO

-- Insert sample data
INSERT INTO Categories (CategoryName, Description)
VALUES 
('Skincare', 'Facial cleansers, moisturizers, and treatments'),
('Makeup', 'Foundations, lipsticks, and eye makeup'),
('Haircare', 'Shampoos, conditioners, and styling products'),
('Fragrances', 'Perfumes and colognes'),
('Bath & Body', 'Soaps, lotions, and body washes');
GO

-- Insert sample products
INSERT INTO Products (ProductName, CategoryId, Description, Price, StockQuantity, ImageUrl)
VALUES
('Hydrating Facial Cleanser', 1, 'Gentle cleanser for all skin types', 24.99, 100, 'pack://application:,,,/CosmeticsStore.WPF;component/Images/cleanser.jpg'),
('Vitamin C Serum', 1, 'Brightening serum with 20% vitamin C', 49.99, 75, 'pack://application:,,,/CosmeticsStore.WPF;component/Images/serum.jpg'),
('Matte Foundation', 2, 'Long-lasting foundation with SPF 15', 34.99, 50, 'pack://application:,,,/CosmeticsStore.WPF;component/Images/foundation.jpg'),
('Volumizing Mascara', 2, 'Adds volume and length to lashes', 19.99, 60, 'pack://application:,,,/CosmeticsStore.WPF;component/Images/mascara.jpg'),
('Moisturizing Shampoo', 3, 'Hydrates and nourishes hair', 15.99, 80, 'pack://application:,,,/CosmeticsStore.WPF;component/Images/shampoo.jpg'),
('Floral Eau de Parfum', 4, 'Light floral fragrance with jasmine notes', 59.99, 40, 'pack://application:,,,/CosmeticsStore.WPF;component/Images/perfume.jpg'),
('Nourishing Body Lotion', 5, 'Rich moisturizer for dry skin', 22.99, 90, 'pack://application:,,,/CosmeticsStore.WPF;component/Images/lotion.jpg'),
('Test Default Image', 5, 'Wrong image package uri', 22.99, 90, 'con.jpg'),
('Test Zero Stock', 5, 'Cannot add to Cart', 25.00, 0, 'con.jpg')
GO

-- Create a manager account
INSERT INTO Users (Username, Password, Email, Role)
VALUES ('admin', '123', 'admin@gmail.com', 'Manager');
GO

-- Create a customer account
INSERT INTO Users (Username, Password, Email, Role)
VALUES ('customer', '123', 'customer@gmail.com', 'Customer');
GO