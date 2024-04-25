using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RWA_Web_API_.Controllers;
using RWA_Web_API_.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
 

        services.AddDbContext<VideoContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Name=ConnectionStrings:ConnectionStringName")));

        services.AddScoped<VideoController>();
    }




    //public void ConfigureServices(IServiceCollection services)
    //{
    //    // Add your services here
    //    // Example: services.AddMvc();

    //    // Configure JWT authentication
    //    var key = Encoding.ASCII.GetBytes("your-secret-key"); // Replace with a secure key
    //    services.AddAuthentication(auth =>
    //    {
    //        auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //        auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //    }).AddJwtBearer(jwt =>
    //    {
    //        jwt.RequireHttpsMetadata = false;
    //        jwt.SaveToken = true;
    //        jwt.TokenValidationParameters = new TokenValidationParameters
    //        {
    //            ValidateIssuerSigningKey = true,
    //            IssuerSigningKey = new SymmetricSecurityKey(key),
    //            ValidateIssuer = false,
    //            ValidateAudience = false
    //        };
    //    });

    //    // Other configurations
    //}

    public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "text/plain";

                    var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>();

                    if (exceptionHandlerPathFeature?.Error != null)
                    {
                        // Log the exception
                        Console.WriteLine($"Exception: {exceptionHandlerPathFeature.Error}");

                        await context.Response.WriteAsync($"An error occurred: {exceptionHandlerPathFeature.Error.Message}");
                    }
                });
            });
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

     
    }

}
