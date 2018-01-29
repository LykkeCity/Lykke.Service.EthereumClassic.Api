﻿using System.Net.Http;
using Autofac;
using Lykke.Service.EthereumClassicApi.Services.Interfaces;

namespace Lykke.Service.EthereumClassicApi.Services
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<GasPriceOracleService>()
                .As<IGasPriceOracleService>()
                .SingleInstance();

            builder
                .RegisterType<AddressValidationService>()
                .As<IAddressValidationService>()
                .SingleInstance();

            builder
                .RegisterType<TransactionStateService>()
                .As<ITransactionStateService>()
                .SingleInstance();

            builder
                .RegisterType<HttpClient>()
                .AsSelf();
        }
    }
}
