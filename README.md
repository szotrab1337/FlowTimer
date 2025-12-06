# Flow Timer

![.NET Version](https://img.shields.io/badge/.NET-10.0-512BD4)
![License](https://img.shields.io/badge/license-MIT-blue)

A modern desktop application for time tracking and productivity management built with WPF and Clean Architecture
principles.

![Application](https://github.com/user-attachments/assets/a9ed8758-3aec-4f4a-8da3-c69ef25d79a1)

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Technologies](#technologies)
- [Getting Started](#getting-started)
- [Screenshots](#screenshots)
- [License](#license)

## 🎯 Overview

Flow Timer is a desktop application designed to help users track their work sessions and improve productivity. Built
with modern .NET technologies and following Clean Architecture principles, the application provides a robust and
maintainable solution for time management.

## ✨ Features

- **Session Tracking** - Track work sessions with start/stop functionality
- **Data Persistence** - All session data is stored locally using SQLite database
- **Logging** - Comprehensive logging system using Serilog
- **Modern UI** - Clean and intuitive WPF interface
- **Theme Support** - Light and dark theme options
- **MVVM Architecture** - Implements MVVM pattern using CommunityToolkit.Mvvm
- **Lightweight** - Minimal resource usage with local data storage

## 🏗️ Architecture

The application follows Clean Architecture principles with clear separation of concerns:

```
FlowTimer/
├── FlowTimer.Domain/          # Entities and repository interfaces
├── FlowTimer.Application/     # Business logic and services
├── FlowTimer.Infrastructure/  # Data access (EF Core + SQLite)
└── FlowTimer.Wpf/             # User interface
```

### Architecture Layers

- **Domain Layer** - Contains core entities and repository interfaces
- **Application Layer** - Implements business logic and application services
- **Infrastructure Layer** - Handles data persistence, logging, and external dependencies
- **Presentation Layer** - WPF-based user interface with MVVM pattern

## 🛠️ Technologies

### Core Framework

- **.NET 10.0** - Latest .NET framework
- **WPF** - Windows Presentation Foundation for desktop UI
- **C# 14.0** - Latest C# language features

### Key Libraries & Packages

#### Presentation Layer

- **CommunityToolkit.Mvvm** (8.4.0) - MVVM implementation

#### Data Access

- **Entity Framework Core** (10.0.0) - ORM for data access
- **SQLite** (10.0.0) - Lightweight database engine

#### Logging

- **Serilog** (4.3.0) - Structured logging framework

## 🚀 Getting Started

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/FlowTimer.git
   cd FlowTimer
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run --project FlowTimer.Wpf
   ```

### First Run

On the first run, the application will:

- Create a SQLite database in `%AppData%\Flow Timer\`
- Initialize logging in the `Logs` directory
- Set up the default configuration

## 📸 Screenshots

![Projects](https://github.com/user-attachments/assets/d8da7e2d-8967-43f9-91a9-022cf0e82eed)
![Project tasks with active session](https://github.com/user-attachments/assets/c31e5c06-45cd-4198-8dae-6633c5b1e103)
![Task sessions list](https://github.com/user-attachments/assets/660cb84f-4553-42e2-a7ef-90d17a9e553b)
![Always on top compact window](https://github.com/user-attachments/assets/ff42f576-2ac8-4ac3-984c-5413446aa9b4)
![Settings](https://github.com/user-attachments/assets/e25ef0ca-4914-4535-81cd-302d0abde865)


## 📝 Roadmap

- Add unit tests for all layers
- Implement export functionality (CSV, PDF)
- Cloud synchronization support
- Multi-language support
- Break reminders and Pomodoro technique
- Statistics and reporting enhancements

## 📄 License

This project is licensed under the MIT License.
