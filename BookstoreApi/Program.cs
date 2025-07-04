using BookstoreApi.Data;
using BookstoreApi.Mapping;
using BookstoreApi.Repositories.Implementations;
using BookstoreApi.Repositories.Interfaces;
using BookstoreApi.Services.Implementations;
using BookstoreApi.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookstoreApi
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
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

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(Program), typeof(AutoMapperProfile));
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IBookService, BookService>();

            var xmlPath = builder.Configuration.GetValue<string>("XmlDataPath");

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();

            app.UseMiddleware<BookstoreApi.Middleware.ErrorHandlingMiddleware>();

            app.UseCors("AllowAll");

            app.Run();
        }
    }
}
