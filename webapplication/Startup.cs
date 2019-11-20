using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using webapplication.Models;
using webapplication.Services;

namespace webapplication
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
			services.AddTransient<IUserService, UserService>();
			services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
			services.AddSignalR();
			services.AddCors(options =>
			{
				options.AddPolicy("EnableCORS", builder =>
				{
					builder.					
					AllowAnyOrigin().
					AllowAnyHeader().
					AllowAnyMethod().
					WithOrigins("http://localhost:4200").
					AllowCredentials();
				});
			});
			services.AddScoped<DBUserContext>();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = "http://localhost:5000",
                    ValidAudience = "http://localhost:5000",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
                };
				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var accessToken = context.Request.Query["access_token"];

						// если запрос направлен хабу
						var path = context.HttpContext.Request.Path;
						if (!string.IsNullOrEmpty(accessToken) &&
							 (path.StartsWithSegments("/chat")))
						{
							// получаем токен из строки запроса
							context.Token = accessToken;
						}
						return Task.CompletedTask;
					}
				};
			});
			
			

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
            }
			app.UseCors("EnableCORS");
			app.UseAuthentication();
			app.UseSignalR(routes =>
			{
				routes.MapHub<ChatHub>("/chat");
			});

			app.UseHttpsRedirection();
			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseMvc();
        }
    }
}
