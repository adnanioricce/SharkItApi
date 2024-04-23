using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Shark.API.Testing.Tools;
//public class TestWebApplicationFactory
//    : WebApplicationFactory<Program>
//{    
//    protected override IHost CreateHost(IHostBuilder builder)
//    {        
//        IConfiguration configuration = null;
//        builder
        
//        .ConfigureAppConfiguration(config => {
//            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
//            configuration = config.AddJsonFile($"appsettings.{env}.json",false)
//            // .AddCommandLine()
//            .AddEnvironmentVariables()
//            // .AddUserSecrets()
//            .Build();
//        })
//        .ConfigureServices(services =>
//        {
//            if(configuration is null){
//                throw new InvalidOperationException("Configuration is not set. Check if the appsettings file is present, or if there any environment variable missing");
//            }
//            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

//            if (descriptor != null)
//            {
//                services.Remove(descriptor);
//            }
//            services.AddDbContext<ApplicationDbContext>((options) =>
//            {
//                var connStr = configuration.GetConnectionString("Default");
//                options.UseNpgsql(connStr);
//            });
//        });

//        return base.CreateHost(builder);
//    }
//}