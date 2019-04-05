using FluentValidation.AspNetCore;
using Marketing.Api.Validators;
using Marketing.Domain.Services;
using Marketing.Persistence.DbContexts;
using Marketing.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Marketing.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            RegisterServices(services);

            services.Configure<MvcOptions>(options => options.Filters.Add(new RequireHttpsAttribute()));

            services.AddDbContext<MarketingDbContext>(options =>
                GetDbContextOptions(options));

            services.AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AdvertisementEntryValidator>());


            services.AddApiVersioning();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Marketing API", Version = "v1"});
                c.CustomSchemaIds(x => x.FullName);
            });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Marketing API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }

        protected virtual DbContextOptionsBuilder GetDbContextOptions(DbContextOptionsBuilder options)
        {
            return options.UseSqlServer(Configuration.GetConnectionString("Marketing"));
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<IAdvertisementService>()
                .AddClasses()
                .AsImplementedInterfaces()
                .WithTransientLifetime());
            services.Scan(scan => scan
                .FromAssemblyOf<AdvertisementRepository>()
                .AddClasses()
                .AsImplementedInterfaces()
                .WithTransientLifetime());
            services.AddSingleton<IConfiguration>(Configuration);
        }
    }
}
