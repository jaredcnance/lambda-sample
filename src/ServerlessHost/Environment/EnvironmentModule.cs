using System;
using System.IO;
using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;

public class EnvironmentModule : Autofac.Module
{
    private const string ENV_CONFIG_KEY = "N2_ENVIRONMENT";
    private const Environment DEFUALT_ENVIRONMENT = Environment.Local;
    protected readonly IConfiguration Config;

    public EnvironmentModule()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();

        Config = builder.Build();
    }

    protected override void Load(ContainerBuilder builder)
    {
        var env = GetEnvironment();
        Console.WriteLine($"Starting in environment: {env}");
        switch(env)
        {
            case Environment.Local:
                RegisterTypesWithAttribute<LocalAttribute>(builder);
                break;
            case Environment.Test:
                RegisterTypesWithAttribute<TestAttribute>(builder);
                break;
            case Environment.Lambda:
                RegisterTypesWithAttribute<LambdaAttribute>(builder);
                break;
            default:
                throw new InvalidOperationException($"Unexpected environment '{env}'");
        }
    }

    private void RegisterTypesWithAttribute<T>(ContainerBuilder builder) where T : Attribute
    {
        foreach(var assembly in ServiceProvider.ServiceAssemblies)
        {
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.GetTypeInfo().IsDefined(typeof(T)))
                .AsImplementedInterfaces();
        }
    }

    private Environment GetEnvironment()
    {
        if(Enum.TryParse(Config[ENV_CONFIG_KEY], out Environment result))
            return result;

        return DEFUALT_ENVIRONMENT;
    }
}