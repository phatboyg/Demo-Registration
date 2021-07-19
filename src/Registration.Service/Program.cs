namespace Registration.Service
{
    using System.Reflection;
    using System.Threading.Tasks;
    using Components.Activities.EventRegistration;
    using Components.Activities.LicenseVerification;
    using Components.Activities.ProcessPayment;
    using Components.Consumers;
    using Components.StateMachines;
    using Data;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;


    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
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

                        x.AddConsumer<SubmitRegistrationConsumer>();
                        x.AddConsumer<ProcessRegistrationConsumer>();

                        x.AddExecuteActivity<LicenseVerificationActivity, LicenseVerificationArguments>();
                        x.AddActivity<EventRegistrationActivity, EventRegistrationArguments, EventRegistrationLog>();
                        x.AddActivity<ProcessPaymentActivity, ProcessPaymentArguments, ProcessPaymentLog>();

                        x.AddSagaStateMachine<RegistrationStateMachine, RegistrationStateInstance>()
                            .EntityFrameworkRepository(r =>
                            {
                                r.ExistingDbContext<RegistrationDbContext>();
                                r.UseSqlServer();
                            });

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ConfigureEndpoints(context);
                        });
                    });

                    services.AddMassTransitHostedService(true);
                });
    }
}