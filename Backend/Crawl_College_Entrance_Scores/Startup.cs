using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crawl_College_Entrance_Scores.Helpers;
using Crawl_College_Entrance_Scores.Repository;
using Crawl_College_Entrance_Scores.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Crawl_College_Entrance_Scores
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
			services.AddDbContext<EntranceScoresContext>(options =>options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			// configure basic authentication 
			services.AddAuthentication("BasicAuthentication")
				.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

			// configure DI for application services
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IUserRepository, UserRepository>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			// global cors policy
			app.UseCors(x => x
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials());

			app.UseAuthentication();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
