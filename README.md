# Bookstore API 📚

## Overview
Bookstore API is a simple ASP.NET Core Web API designed to manage a collection of books stored in an XML file.  
The API supports adding, updating, deleting, and retrieving books by their ISBN.  
It also provides the ability to generate an HTML report listing all books.

---

## Features

- CRUD operations on books identified by ISBN
- Support for multiple authors per book
- Validation of input data using Data Annotations
- Asynchronous operations for efficient file handling
- Global error handling with clear messages
- AutoMapper integration for DTO mapping
- Swagger/OpenAPI documentation for easy testing
- Configurable XML data file path per environment

---

## Technologies Used

- ASP.NET Core 8 Web API
- C# 11
- LINQ to XML (`XDocument`)
- AutoMapper
- Swagger (Swashbuckle)

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Running the API

1. Clone the repository:

```bash
git clone https://github.com/michaeleliyahu/BookSrote.git
cd BookstoreApi
```

2. Configure the path to the XML data file in appsettings.json (default is Data/bookstore.xml):

```bash
{
  "XmlDataPath": "BookstoreAp/Data/bookstore.xml"
}
```

3. Run the API:

```bash
dotnet run --project BookstoreApi
```

4. When running the project, the Swagger UI will automatically open in your browser at:

```bash
http://localhost:5095/swagger/index.html
```
