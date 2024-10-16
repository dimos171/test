# Santander - Developer Coding Test

This is a simple .NET Core RESTful API that retrieves details of the first `n` "best stories" from the Hacker News API, where `n` is specified by the caller. The solution follows **DDD** (Domain-Driven Design) and **SOLID** principles.

## Solution Overview

The solution consists of:
- **Executable .NET API project**
- **Contact and Service libraries**
- **Unit Test project**

## Implementation Details

### API Functionality
The API provides functionality to retrieve stories with **pagination support** through `take` and `skip` parameters (I added the `skip` parameter to simulate real-world paging).

### Service Layer Highlights
1. **Multithreading**  
   Utilizes `Task.WhenAll()` for parallel HTTP calls to fetch story details efficiently.

2. **Caching with `ConcurrentDictionary`**  
   Caches responses to reduce repeated calls to the Hacker News API and avoid overloading it.

3. **Retry Mechanism**  
   Automatically retries requests to handle temporary issues with the Hacker News API.

### API Layer Features
1. **Custom Global Error Handling Middleware**  
   Handles exceptions and provides consistent error responses.

2. **Dependency Injection**  
   Uses standard .NET DI to manage dependencies for better testability and separation of concerns.

### Unit Testing
- I included a **Unit Test project** to demonstrate how we can write unit tests to validate the API logic.
- Not all parts of the code are fully tested, but I aimed to highlight the importance of unit testing and demonstrate basic test practices.

## TODOs

Throughout the code, I’ve added several `TODO` comments with ideas for improvements that are **out of scope** for this task.