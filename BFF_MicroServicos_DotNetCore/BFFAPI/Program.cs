using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Confluent.Kafka;
using BFFAPI.Application.Services.ClienteWEB;
using BFFAPI.Application.Services.PagamentoWEB;
using BFFAPI.Application.Services;
using BFFAPI.Services.Autenticacao;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using BFFAPI.Application.Services.Autenticacao;
using BFFAPI.Application.Services.Mensageria;

namespace BFFAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     config.AddJsonFile("appsettings.json", optional: true);
                 })
                .ConfigureServices((hostContext, services) =>
                {                    
                    var jwtSecretKey = hostContext.Configuration["JwtSecretKey"];
                    services.AddScoped<IAuthService>(provider => new AuthService(jwtSecretKey));
                    services.AddScoped<IJwtAuthService>(provider => new JwtAuthService(jwtSecretKey));
                    
                    services.AddHttpContextAccessor();                    
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));

                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    }).AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = key,
                            ValidateIssuer = false, 
                            ValidateAudience = false
                        };
                    });

                    services.Configure<KafkaSettings>(hostContext.Configuration.GetSection("KafkaSettings"));
                    services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((hostContext, services) =>
                    {
                        services.AddHttpClient();
                        services.AddScoped<IClienteBFFService, ClienteBFFService>();
                        services.AddScoped<IPagamentoService, PagamentoService>();
                        services.AddScoped<IClientePagamentoService, ClientePagamentoService>();                        
                        
                        services.AddSingleton<IProducer<Null, string>>(provider =>
                        {
                            var config = new ProducerConfig
                            {
                                BootstrapServers = "localhost:9092",
                            };

                            return new ProducerBuilder<Null, string>(config).Build();
                        });

                        services.AddCors(options =>
                        {
                            options.AddPolicy("CorsPolicy", builder =>
                            {
                                builder.AllowAnyOrigin()
                                       .AllowAnyHeader()
                                       .AllowAnyMethod()
                                       .WithExposedHeaders("Authorization");
                            });
                        });

                        services.AddControllers();
                        services.AddEndpointsApiExplorer();
                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new OpenApiInfo { Title = "BFF API", Version = "v1" });                            
                            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                            {
                                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                                Name = "Authorization",
                                In = ParameterLocation.Header,
                                Type = SecuritySchemeType.ApiKey,
                                Scheme = "Bearer"
                            });                            
                            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                           {
                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"
                                        }
                                    },
                                    new string[] {}
                                }
                            });
                        });
                    });

                    webBuilder.Configure(app =>
                    {                        
                        app.UseCors("CorsPolicy");

                        app.UseSwagger();
                        app.UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/swagger/v1/swagger.json", "BFF API V1");
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
