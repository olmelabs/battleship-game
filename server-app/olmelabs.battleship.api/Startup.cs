using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using olmelabs.battleship.api.BackgroundServices;
using olmelabs.battleship.api.Logic;
using olmelabs.battleship.api.Models;
using olmelabs.battleship.api.Repositories;
using olmelabs.battleship.api.Services.Implementations;
using olmelabs.battleship.api.Services.Interfaces;
using olmelabs.battleship.api.SignalRHubs;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace olmelabs.battleship.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<GameOptions>(Configuration);

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            services.AddAutoMapper();

            /*
                After updating to signalr 1.1.0 withCredentials heade is hardocded to true in js. 
                Had to send origin in response. 
                AllowAnyOrigin sends * in response which is not acceptable when withCredentials = true
            */
            services.AddCors(options =>
            {
                options.AddPolicy("AllowedOrigins",
                    builder =>
                    {
                        builder
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        //.AllowAnyOrigin()
                        .WithOrigins(Configuration["Cors:AllowedOrigins"].Split(","))
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            if (Configuration["Storage"] == "InMemory")
            {
                services.AddTransient<IStorage, InMemoryStaticStorage>();
            }
            else if (Configuration["Storage"] == "MongoDb")
            {
                //https://mongodb.github.io/mongo-csharp-driver/2.7/reference/driver/connecting/
                //It is recommended to store a MongoClient instance in a global place, either as a static variable or in an IoC container with a singleton lifetime.
                services.AddSingleton<IStorage, MongoDbStorage>();
            }
            else
            {
                throw new InvalidOperationException("Storage is not configured.");
            }
            services.AddTransient<IRazorViewRendererService, RazorViewRendererService>();
            services.AddTransient<IGameStatisticsService, GameStatisticsService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IPeerToPeerGameService, PeerToPeerGameService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IGameLogic, GameLogic>();

            services.AddSingleton<IHostedService, MailerService>();
            services.AddSingleton<IHostedService, PlayerService>();
            services.AddSingleton<IHostedService, StatisticsCollectorService>();
            services.AddSingleton<IHostedService, CleanupService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier, //jwt sub claim converted to this. (crazy ?),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });


            services.AddMvc();

            services.AddSignalR();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Omelabs Battle Ship Game API V1", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            IStorage storage = app.ApplicationServices.GetService<IStorage>();
            Task.Run(() => storage.Prepare());

            app.UseCors("AllowedOrigins");

            app.UseAuthentication();

            app.UseMvc();

            app.UseSignalR(routes =>
            {
                routes.MapHub<GameHub>("/game-hub");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Omelabs Battle Ship Game API V1");
            });
        }
    }
}
