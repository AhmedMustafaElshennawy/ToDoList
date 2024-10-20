# Task Management API

## Overview

This API is built using ASP.NET Core and is designed to manage users, roles, to-do lists, and tasks. The system incorporates authentication and authorization mechanisms using ASP.NET Identity. The core entities include users, roles, tasks, and user claims, allowing for a structured task management system.

## Features

- **User Authentication & Authorization**: Users can sign up, log in, and manage their roles.
- **To-Do List Management**: Each user can create and manage their personal to-do lists.
- **Task Management**: Tasks can be created, updated, and linked to specific to-do lists.
- **Role-Based Access Control**: User roles are managed via claims and roles, defining access levels.

## Entities

### 1. **Users**
- **Id**: Unique identifier for the user.
- **UserName**: The username for the account.
- **Email**: The user's email address.
- **PasswordHash**: Stores the hashed password.

### 2. **ToDoList**
- **Id**: Unique identifier for the to-do list.
- **ApplicationUserId**: The ID of the user who owns the to-do list.

### 3. **Tasks**
- **Id**: Unique identifier for a task.
- **Description**: Detailed description of the task.
- **CreatedOn**: Timestamp of when the task was created.
- **ToDoListId**: Foreign key linking a task to a specific to-do list.

## Database Schema

The database consists of the following tables:
![Alt text](https://github.com/AhmedMustafaElshennawy/ToDoList/blob/master/Screenshot%202024-10-17%20134755.png)

## Authentication & Authorization

- The API uses JWT (JSON Web Tokens) for authentication.
- Role-based authorization is implemented to restrict access to certain endpoints.

