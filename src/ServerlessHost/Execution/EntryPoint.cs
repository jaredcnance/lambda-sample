using System;
using System.Reflection;
using Autofac;

public interface IExecutable<in TIn>
{
    void Execute(TIn input);
}

public interface IExecutable<in TIn, out TOut>
{
    TOut Execute(TIn input);
}

public class EntryPoint<TService, TIn>
    where TService : IExecutable<TIn>
{
    private readonly Assembly[] _serviceAssemblies;

    public EntryPoint(params Assembly[] serviceAssemblies) 
    {
        _serviceAssemblies = serviceAssemblies;
    }

    public void Run(TIn request)
    {
        ServiceProvider<TService>.ServiceAssemblies = _serviceAssemblies;
        using (var scope = ServiceProvider<TService>.Container.BeginLifetimeScope())
            scope.Resolve<TService>().Execute(request);
    }
}

public class EntryPoint<TService, TIn, TOut>
    where TService : IExecutable<TIn, TOut>
{
    private readonly Assembly[] _serviceAssemblies;

    public EntryPoint(params Assembly[] serviceAssemblies) 
    {
        _serviceAssemblies = serviceAssemblies;
    }
    
    public TOut Run(TIn request)
    {
        ServiceProvider<TService>.ServiceAssemblies = _serviceAssemblies;
        using (var scope = ServiceProvider<TService>.Container.BeginLifetimeScope())
            return scope.Resolve<TService>().Execute(request);
    }
}