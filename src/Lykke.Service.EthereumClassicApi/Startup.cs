﻿using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Log;
using Lykke.Service.EthereumClassicApi.Actors;
using Lykke.Service.EthereumClassicApi.Blockchain;
using Lykke.Service.EthereumClassicApi.Common.Settings;
using Lykke.Service.EthereumClassicApi.Modules;
using Lykke.Service.EthereumClassicApi.Repositories;
using Lykke.Service.EthereumClassicApi.Services;
using Lykke.Service.EthereumClassicApi.Utils;
using Lykke.SettingsReader;
using Lykke.SlackNotifications;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;


namespace Lykke.Service.EthereumClassicApi
{
    public class Startup
    {
        private readonly IHostingEnvironment            _environment;
        private readonly IReloadingManager<AppSettings> _settings;
        private readonly ILog                           _log;
        private readonly ISlackNotificationsSender      _notificationsSender;


        private IContainer _container;


        public Startup(IHostingEnvironment environment)
        {
            _environment = environment;
            _settings    = LoadSettings();

            (_log, _notificationsSender) = LykkeLoggerFactory.CreateLykkeLoggers(_settings);
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            try
            {
                app
                    .UseMvc()
                    .UseSwagger(SetupSwagger)
                    .UseSwaggerUI(SetupSwaggerUI)
                    .UseStaticFiles();
            }
            catch (Exception e)
            {
                WriteFatalError(e, nameof(Configure));

                throw;
            }
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            try
            {
                services
                    .AddMvc();
                
                services
                    .AddSwaggerGen(SetupSwaggerGen);

                var builder = new ContainerBuilder();
                
                builder
                    .RegisterModule(new SettingsModule(_settings))
                    .RegisterModule(new LoggerModule(_log, _notificationsSender))
                    .RegisterModule<ActorsModule>()
                    .RegisterModule<BlockchainModule>()
                    .RegisterModule<RepositoriesModule>()
                    .RegisterModule<ServicesModule>();

                builder
                    .Register(ctx => ActorSystemFacadeFactory.Build(_container))
                    .As<IActorSystemFacade>()
                    .SingleInstance();

                builder
                    .Populate(services);
                
                _container = builder.Build();
                
                
                _container
                    .Resolve<IActorSystemFacade>();

                return new AutofacServiceProvider(_container);
            }
            catch (Exception e)
            {
                WriteFatalError(e, nameof(ConfigureServices));
                
                throw;
            }
        }

        private IReloadingManager<AppSettings> LoadSettings()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(_environment.ContentRootPath)
                .AddEnvironmentVariables()
                .Build();

            return configuration.LoadSettings<AppSettings>();
        }

        private void WriteFatalError(Exception e, string process)
        {
            _log.WriteFatalErrorAsync
            (
                nameof(Startup),
                process,
                "",
                e
            ).GetAwaiter().GetResult();
        }

        private static void SetupSwagger(SwaggerOptions options)
        {
            options.PreSerializeFilters.Add
            (
                (swagger, httpReq) => swagger.Host = httpReq.Host.Value
            );
        }

        private static void SetupSwaggerGen(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new Info { Title = "Ethereum Classic API", Version = "v1" });
        }

        private static void SetupSwaggerUI(SwaggerUIOptions options)
        {
            options.RoutePrefix = "swagger/ui";

            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        }
    }
}