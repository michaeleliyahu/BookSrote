using BookstoreApi.Data;
using BookstoreApi.Mapping;
using BookstoreApi.Repositories.Implementations;
using BookstoreApi.Repositories.Interfaces;
using BookstoreApi.Services.Implementations;
using BookstoreApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add controllers service (important for API controllers)
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AutoMapper services
builder.Services.AddAutoMapper(typeof(Program), typeof(AutoMapperProfile));

// Register repositories and services for dependency injection
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();


// Load XML file path from configuration
var xmlPath = builder.Configuration.GetValue<string>("XmlDataPath");

// Build the WebApplication
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Map controller endpoints
app.MapControllers();

app.UseMiddleware<BookstoreApi.Middleware.ErrorHandlingMiddleware>();

app.UseCors("AllowAll");

app.Run();
