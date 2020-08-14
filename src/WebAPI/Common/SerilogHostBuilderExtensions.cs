using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Debugging;
using Serilog.Extensions.Logging;

public static class SerilogHostBuilderExtensions
{
    public static IHostBuilder UseSerilog(this IHostBuilder builder,
        Serilog.ILogger logger = null, bool dispose = false)
    {
        builder.ConfigureServices((context, collection) =>
            collection.AddSingleton<ILoggerFactory>(services => new SerilogLoggerFactory(logger, dispose)));
        return builder;
    }
}

public class SerilogLoggerFactory : ILoggerFactory
{
    private readonly SerilogLoggerProvider _provider;

    public SerilogLoggerFactory(Serilog.ILogger logger = null, bool dispose = false)
    {
        _provider = new SerilogLoggerProvider(logger, dispose);
    }

    public void Dispose() => _provider.Dispose();

    public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
    {
        return _provider.CreateLogger(categoryName);
    }

    public void AddProvider(ILoggerProvider provider)
    {
        // Only Serilog provider is allowed!
        SelfLog.WriteLine("Ignoring added logger provider {0}", provider);
    }
}
