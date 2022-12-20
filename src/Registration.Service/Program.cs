using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Registration.Components;
using Registration.Data;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("MassTransit", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<RegistrationDbContext>(r =>
        {
            var connectionString = hostContext.Configuration.GetConnectionString("Sagas");

            r.UseSqlServer(connectionString, m =>
            {
                m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                m.MigrationsHistoryTable($"__{nameof(RegistrationDbContext)}");
            });
        });

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.SetEntityFrameworkSagaRepositoryProvider(r =>
            {
                r.ExistingDbContext<RegistrationDbContext>();
                r.UseSqlServer();
            });

            x.AddConsumersFromNamespaceContaining<ComponentsNamespace>();
            x.AddActivitiesFromNamespaceContaining<ComponentsNamespace>();
            x.AddSagaStateMachinesFromNamespaceContaining<ComponentsNamespace>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
    }).Build();

await host.RunAsync();