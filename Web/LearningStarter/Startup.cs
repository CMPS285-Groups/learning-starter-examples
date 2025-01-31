using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningStarter.Data;
using LearningStarter.Entities;
using LearningStarter.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LearningStarter
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
            services.AddCors();
            services.AddControllers();

            services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.MaxValue;
                options.Preload = true;
                options.IncludeSubDomains = true;
            });

            services.AddDbContext<DataContext>(options =>
            {
                // options.UseInMemoryDatabase("FooBar");
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //TODO
            services.AddMvc();

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };
                });

            services.AddAuthorization();

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Learning Starter Server",
                    Version = "v1",
                    Description = "Description for the API goes here.",
                });

                c.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : null);
                c.MapType(typeof(IFormFile), () => new OpenApiSchema { Type = "file", Format = "binary" });
            });

            services.AddSpaStaticFiles(config =>
            {
                config.RootPath = "learning-starter-web/build";
            });

            services.AddHttpContextAccessor();

            // configure DI for application services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext dataContext)
        {
            dataContext.Database.EnsureDeleted();
            dataContext.Database.EnsureCreated();
            
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(options =>
            {
                options.SerializeAsV2 = true;
            }); ;

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Learning Starter Server API V1");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(x => x.MapControllers());

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "learning-starter-web";
                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3001");
                }
            });

            SeedUsers(dataContext);
            SeedOrders(dataContext);
            SeedWorkouts(dataContext);
            SeedUserWorkouts(dataContext);
        }

        private void SeedUserWorkouts(DataContext dataContext)
        {
            if (!dataContext.UserWorkouts.Any())
            {
                var user = dataContext.Users.First();
                var workout1 = dataContext.Workouts.First();

                var seededUserWorkout = new UserWorkout
                {
                    User = user,
                    Workout = workout1,
                };

                dataContext.UserWorkouts.Add(seededUserWorkout);
                dataContext.SaveChanges();
            }
        }

        private void SeedWorkouts(DataContext dataContext)
        {
            var seededWorkoutType = new WorkoutType
            {
                Name = "Cardio",
            };

            if (!dataContext.Workouts.Any())
            {
                var workoutsToSeed = new List<Workout>
                {
                    new Workout
                    {
                        Name = "Push-Ups",
                        WorkoutType = seededWorkoutType,
                    },
                    new Workout
                    {
                        Name = "Sit-Ups",
                        WorkoutType = seededWorkoutType,
                    }
                };

                dataContext.Workouts.AddRange(workoutsToSeed);
                dataContext.SaveChanges();
            }
        }

        private void SeedOrders(DataContext dataContext)
        {
            if (!dataContext.Orders.Any())
            {
                var seededOrder = new Order
                {
                    DatePurchased = DateTimeOffset.Now,
                    PaymentType = "Credit"
                };

                dataContext.Orders.Add(seededOrder);

                var seededProduct = new Product()
                {
                    Cost = 100,
                    Name = "Beans",
                    Quantity = 1,
                };

                dataContext.Products.Add(seededProduct);

                var seededOrderProduct = new OrderProduct
                {
                    Order = seededOrder,
                    Product = seededProduct,
                    Quantity = 5,
                    Price = 500,
                };

                dataContext.OrderProducts.Add(seededOrderProduct);

                dataContext.SaveChanges();
            }
        }

        public void SeedUsers(DataContext dataContext)
        {
            var numUsers = dataContext.Users.Count();

            if (numUsers == 0)
            {
                var seededUser = new User
                {
                    FirstName = "Seeded",
                    LastName = "User",
                    Username = "admin",
                    Password = "password"
                };

                dataContext.Users.Add(seededUser);
                dataContext.SaveChanges();
            }
        }
    }
}
