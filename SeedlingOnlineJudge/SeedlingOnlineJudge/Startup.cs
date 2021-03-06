using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeedlingOnlineJudge.Database;
using SeedlingOnlineJudge.Web.Filters;
using SeedlingOnlineJudge.Infrastructure.File;
using SeedlingOnlineJudge.Util;
using SeedlingOnlineJudge.BO;

namespace SeedlingOnlineJudge
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOriginsPolicy", // I introduced a string constant just as a label "AllowAllOriginsPolicy"
                builder =>
                {
                    builder.AllowAnyOrigin();
                });
            });

            services.AddDataProtection();

            services.AddControllers();

            // MANAGERS
            services.AddSingleton<FileManager>();
            services.AddSingleton<ProblemsManager>();
            services.AddSingleton<UserManager>();
            services.AddSingleton<IDatabase>();

            // BO
            services.AddSingleton<SolutionBO>();
            services.AddSingleton<ContractBO>();

            // Util
            services.AddSingleton<Cipher>();

            // Filters
            services.AddScoped<UserFilter>();
            services.AddScoped<AuthorFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors("AllowAllOriginsPolicy");

            app.UseEndpoints(endpoints => 
            {
                endpoints.MapControllers();
            });
        }
    }
}
