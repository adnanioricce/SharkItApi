using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shark.Domain.CustomerManagement;

namespace Shark.IntegrationTests.Tools;
public delegate object GetService(Type type);
public static class Ioc {
    private static readonly IServiceProvider _sp;
    public static readonly ILogger _logger;
    public static readonly IConfiguration _configuration;    
    static Ioc(){
        // _app = Program.Create(Enumerable.Empty<string>().ToArray());        
        // if(_app is null){
        //     throw new InvalidOperationException($"Couldn't setup the testing dependencies. check the Appplication configuration in {typeof(Program).Name}");
        // }
        var services = new ServiceCollection();        
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(Customer).Assembly));
        services.AddDbContext<ApplicationDbContext>(opt => {            
            opt.UseNpgsql("Host=localhost;Port=5147;Username=sharkuser;Password=sharkpass;Database=sharkdb");
            opt.EnableDetailedErrors();
            opt.EnableSensitiveDataLogging();
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        services.AddLogging();
        _sp = services.BuildServiceProvider();
    }    
    public static object GetServiceWith(IServiceProvider sp,Type serviceType)
    {
        if(sp is null){
            throw new ArgumentNullException(nameof(sp));
        }
        return sp.GetService(serviceType);
    }

    public static T GetServiceWith<T>(IServiceProvider serviceProvider)
        => (T)GetServiceWith(serviceProvider,typeof(T));
    public static T GetService<T>() => GetServiceWith<T>(_sp);    
    
    public static T CreateInstanceWith<T>()
    {
        Type type = typeof(T);
        ConstructorInfo[] constructors = type.GetConstructors();

        if (constructors.Length != 1)
        {
            throw new InvalidOperationException("Type must have exactly one constructor.");
        }

        ConstructorInfo constructor = constructors[0];
        ParameterInfo[] parameters = constructor.GetParameters();

        List<object> arguments = new List<object>();
        foreach (ParameterInfo parameter in parameters)
        {
            Type parameterType = parameter.ParameterType;
            object service = GetServiceWith(_sp,parameterType);
            if (service == null)
            {
                throw new InvalidOperationException($"Unable to resolve service for type '{parameterType}' while attempting to activate '{type}'.");
            }
            arguments.Add(service);
        }

        return (T)Activator.CreateInstance(type, arguments.ToArray());
    }
}