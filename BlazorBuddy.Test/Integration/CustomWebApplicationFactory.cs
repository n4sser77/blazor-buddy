using BlazorBuddy.WebApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BlazorBuddy.Test.Integration
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.UseEnvironment("IntegrationTest");

            builder.ConfigureServices(services =>
            {

                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
                services.RemoveAll(typeof(ApplicationDbContext));


                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TestDatabase_" + Guid.NewGuid()));


                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}
