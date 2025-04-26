# BookShop
📚 Project: E-Commerce Book Store
🎯 Project Idea
An online bookstore where users can browse books, add them to their cart, place orders, and track order statuses.

🏗️ Technologies Used
Backend: ASP.NET Core Web API

Database: SQL Server

ORM: Entity Framework Core

Design Patterns:

Mediator Pattern (MediatR)

CQRS (Command and Query Responsibility Segregation)

Dependency Injection (built-in .NET Core)

Localization (Arabic and English)

FluentValidation (for request validation)

Transaction Management (to ensure data integrity)

Exception Handling (global + validation errors)

🗂️ Project Architecture
API Layer: Controllers (handle HTTP requests)

Core Layer:

Features (Commands, Queries, Handlers, Validations)

DTOs (Request/Response Models)

Mapping Profiles (AutoMapper)

Service Layer:

Interfaces

Implementations

Infrastructure Layer:

Generic Repositories

ApplicationDbContext

Migrations

DataAccess Layer:

Entities (Book, ShoppingCart, CartItem, Order, OrderItem, PaymentMethod, ShippingMethod, Address, etc.)

🔥 Main Entities
User (Application users)

Book (Books for sale)

CartItem (Items in the user's cart)

ShoppingCart (Cart entity)

Order (User order)

OrderItem (Details for each order)

ShippingMethod (Shipping options)

PaymentMethod (Payment options)

Address (Shipping and billing addresses)

OrderState (Order status: Pending, Confirmed, Shipped, etc.)

⚙️ Features Implemented
🛒 Cart Management
Add a book to the cart

Update the quantity of a book in the cart

Remove a book from the cart

Prevent adding the same book multiple times

Validate quantity availability before adding

📦 Order Management
Create a new order

Calculate total price including shipping cost

Deduct stock quantity when order is placed

Change order status (Pending → Confirmed → Shipped → Delivered → Completed)

Cancel an order and restore book stock

Validate order cancellation based on order status

🚚 Shipping Management
Create, update, delete shipping methods

💳 Payment Management
Create, update, delete payment methods

🧾 Address Management
Save address when placing an order

Retrieve user’s addresses

🔐 Security & Authorization
Verify the current user owns the order before editing/canceling

Return localized error messages (Arabic/English)

Unauthorized access protection

✨ Best Practices Followed
Unit of Work inside transactions

AsNoTracking for read-only queries (better performance)

Separate Validation using FluentValidation

Strictly following CQRS (Commands = Write, Queries = Read)

Using DTOs (Data Transfer Objects) to avoid exposing entities

Single Responsibility Principle for services and handlers

Transaction Management for critical operations (order placement)

Navigation Properties properly handled (example: Order → OrderState)

🧩 Main Services Created
IBookService

ICartItemService

IOrderService

IOrderItemService

IAddressService

IShippingMethodService

IPaymentMethodService

IOrderStateService

ICurrentUserService (get current user ID and info)

🛠️ Extra Tools/Technologies
AutoMapper (for DTO mapping)

Localization Resources (for multi-language support)

Global Exception Middleware (custom error responses)

Sending Email Notifications (order confirmation, shipping updates)

Creating a Dashboard for monitoring sales and orders

