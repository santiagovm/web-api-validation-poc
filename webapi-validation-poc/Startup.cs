using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiValidationPoC
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
            // todo: wire up validators in DI container
            services.AddSingleton<IValidator<User>, UserValidator>();
            
            // todo: wire up validators in http pipeline
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddFluentValidation();

            // todo: override default behavior for InvalidModelState management
            services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        List<string> errors = context.ModelState
                                                     .Values
                                                     .SelectMany(modelStateEntry =>
                                                         modelStateEntry.Errors.Select(modelError => modelError.ErrorMessage))
                                                     .ToList();

                        var result = new
                        {
                            Code = "00009",
                            Message = "Validation errors",
                            Errors = errors,
                        };
                        
                        return new BadRequestObjectResult(result);
                    };
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
