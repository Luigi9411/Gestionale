
using Gestionale.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace Gestionale
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AgentiContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MainDb")));

            builder.Services.AddControllers().AddJsonOptions(jsOpt => jsOpt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


      

            builder.Services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(
                    build =>
                    {
                        build.SetIsOriginAllowed(origin => true)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors();


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}