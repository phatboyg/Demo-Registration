namespace Registration.Api
{
    using System.Configuration;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Common;
    using Contracts;
    using MassTransit;
    using MassTransit.AzureServiceBusTransport;


    public class WebApiApplication : HttpApplication
    {
        static IBusControl _busControl;

        public static IBus Bus => _busControl;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            StartServiceBus();
        }

        void StartServiceBus()
        {
            _busControl = MassTransit.Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                var host = cfg.Host(ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"], h =>
                {
                });

                EndpointConvention.Map<SubmitRegistration>(
                    host.Settings.ServiceUri.GetDestinationAddress(ConfigurationManager.AppSettings["SubmitRegistrationQueueName"]));
            });

            _busControl.Start();
        }

        protected void Application_End()
        {
            _busControl?.Stop();
        }
    }
}