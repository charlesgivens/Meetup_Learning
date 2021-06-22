using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using MeetupAPI.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using MeetupApi.Models;
using MeetupApi.Validators;
using MeetupApi.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MeetupAPI.Identity;
using MeetupApi.Provider;

namespace MeetupApi
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
          var jwtOptions = new JwtOptions();
          Configuration.GetSection("jwt").Bind(jwtOptions);

          services.AddSingleton(jwtOptions);

          services.AddAuthentication(options =>
          {
            options.DefaultAuthenticateScheme = "Bearer";
            options.DefaultScheme = "Bearer";
            options.DefaultChallengeScheme = "Bearer";
          }).AddJwtBearer(cfg =>
          {
            cfg.RequireHttpsMetadata = false;
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
              ValidIssuer = jwtOptions.JwtIssuer,
              ValidAudience = jwtOptions.JwtIssuer,
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.JwtKey))
            };
          });

          services.AddScoped<IJwtProvider, JwtProvider>();
          services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
          services.AddControllers().AddFluentValidation();
          services.AddScoped<IValidator<RegisterUserDto>, RegisterUserValidator>();
          services.AddControllers();
          services.AddSwaggerGen(c =>
          {
              c.SwaggerDoc("v1", new OpenApiInfo { Title = "MeetupApi", Version = "v1" });
          });

          services.AddDbContext<MeetupContext>();
          services.AddScoped<MeetupSeeder>();
          services.AddScoped<RoleSeeder>();
          services.AddAutoMapper(this.GetType().Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MeetupSeeder meetupSeeder, RoleSeeder roleSeeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeetupApi v1"));
                meetupSeeder.Seed();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            roleSeeder.Seed();
        }
    }
}
