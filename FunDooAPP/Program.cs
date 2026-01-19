
using BessinessLogicLayer.Helpers;
using BessinessLogicLayer.Interface;
using BessinessLogicLayer.Repository;
using BessinessLogicLayer.Validators;
using DataLogicLayer.Data;
using DataLogicLayer.Interface;
using DataLogicLayer.Repository;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManagement.Api.Middleware;
using UserManagement.BLL.Mapping;

namespace FunDooAPP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ------------------------------
            // 1. Add Controllers
            // ------------------------------
            builder.Services.AddControllers();

            // ------------------------------
            // 2. Add DbContext
            // ------------------------------
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.CommandTimeout(30))
            );

            // ------------------------------
            // 3. Add Repository Pattern
            // ------------------------------
            builder.Services.AddScoped<IUserDL, UserDL>();

            // ------------------------------
            // 4. Add Business Logic Services
            // ------------------------------
            builder.Services.AddScoped<IUserBL, UserBL>();

            // ------------------------------
            // 5. Add AutoMapper
            // ------------------------------
            builder.Services.AddAutoMapper(typeof(UserProfile));

            // ------------------------------
            // 6. Add FluentValidation
            // ------------------------------
            builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
            builder.Services.AddScoped<UserRequestValidator>();

            // ------------------------------
            // 7. Add Swagger/OpenAPI
            // ------------------------------
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter JWT token like: Bearer {your token}"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // ------------------------------
            // 8. Add Logging
            // ------------------------------
            builder.Services.AddLogging(config =>
            {
                config.AddConsole();
                config.AddDebug();
            });

            // ------------------------------
            // 9. Add CORS
            // ------------------------------
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            //Authorization
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer=true,
                    ValidateAudience=true,
                    ValidateLifetime=true,
                    ValidateIssuerSigningKey=true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            builder.Services.AddTransient<IEmailSender, EmailSender>();



            var app = builder.Build();

            // ------------------------------
            // Configure the HTTP request pipeline
            // ------------------------------

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(); // Generates Swagger JSON
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API v1");
                    //c.RoutePrefix = string.Empty; // Swagger at root
                });
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            //Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Global Exception Handling Middleware
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            // Map Controllers
            app.MapControllers();

            app.Run();
        }
    }
}
