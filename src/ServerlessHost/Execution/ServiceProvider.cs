using System;
using System.Reflection;
using Autofac;

public class ServiceProvider
{
    public static Assembly[] ServiceAssemblies { get; internal set; }

    public ServiceProvider(params Assembly[] serviceAssemblies)
    {
        ServiceAssemblies = serviceAssemblies;
    }
}

public sealed class ServiceProvider<TService> : ServiceProvider
{
    private static readonly Lazy<IContainer> lazy = new Lazy<IContainer>(() => Build());
    public static IContainer Container => lazy.Value;

    public ServiceProvider(params Assembly[] serviceAssemblies) 
    : base (serviceAssemblies)
    { }

    private static IContainer Build()
    {
        var builder = new ContainerBuilder();

        builder.RegisterModule<EnvironmentModule>();

        builder.RegisterType<TService>();

        var container = builder.Build();

        return container;
    }
}