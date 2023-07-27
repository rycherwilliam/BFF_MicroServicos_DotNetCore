using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ClientesAPI.Application.Services;
using ClientesAPI.Infrastructure.Repositories;
using ClientesAPI.Infrastructure;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ClientesAPI.Application.Services.Autenticacao;

namespace ClientesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
          
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ClientesContext>();
                try
                {
                    context.Database.EnsureCreated();                    
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Um erro ocorreu ao criar o banco de dados.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((hostContext, services) =>
                    {
                        services.AddDbContext<ClientesContext>(options =>
                        {
                            options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection"));
                        });
                        services.AddHttpClient();
                        services.AddScoped<IClienteService, ClienteService>();
                        services.AddScoped<IClienteRepository, ClienteRepository>();                        
                        var jwtSecretKey = hostContext.Configuration["JwtSecretKey"];
                        services.AddAuthentication(options =>
                        {
                            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        }).AddJwtBearer(options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters
                                {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
                                ValidateIssuer = false,
                                ValidateAudience = false
                                };
                        });
                        services.AddScoped<IJwtAuthService>(provider => new JwtAuthService(jwtSecretKey));                        

                        services.AddControllers();
                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Clientes API", Version = "v1" });
                        });

                        services.AddCors(options =>
                        {
                            options.AddPolicy("AllowBFF", builder =>
                            {
                                builder.WithOrigins("https://localhost:3000")
                                       .AllowAnyHeader()
                                       .AllowAnyMethod()
                                       .WithExposedHeaders("Authorization");
                            });
                        });
                    });

                    webBuilder.Configure(app =>
                    {
                        app.UseCors("AllowBFF");
                        app.UseSwagger();
                        app.UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clientes API V1");
                        });

                        app.UseRouting();
                        app.UseAuthentication();
                        app.UseAuthorization();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });                        
                    });
                });
    }
}
