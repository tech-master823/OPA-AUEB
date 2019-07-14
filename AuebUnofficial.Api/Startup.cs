using AuebUnofficial.Api.Data;
using AuebUnofficial.Api.Interfaces;
using AuebUnofficial.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using Swashbuckle.AspNetCore.Swagger;

namespace AuebUnofficial.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment environment)
        {
            Configuration = new ConfigurationBuilder()
            .SetBasePath(environment.ContentRootPath)
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables()
            .AddUserSecrets<Startup>()
            .Build();
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // Add Nodatime IClock
            services.AddSingleton<IClock>(SystemClock.Instance);
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ISchoolAnnouncementsService, SchoolAnnouncementsService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(options =>
    options.SwaggerDoc("v1", new Info { Title = "AuebUnofficial API", Version = "v1" })
);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseSwagger();

            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auebunofficial API v1")
            );
            app.UseMvc();
        }
    }
}
