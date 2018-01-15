﻿using System;
using System.Threading.Tasks;
using Lykke.Service.EthereumClassicApi.Actors.Roles.Interfaces;
using Lykke.Service.EthereumClassicApi.Repositories.Interfaces;

namespace Lykke.Service.EthereumClassicApi.Actors.Roles
{
    public class TransactionProcessorDispatcherRole : ITransactionProcessorDispatcherRole
    {
        private readonly IBuiltTransactionRepository _builtTransactionRepository;


        public TransactionProcessorDispatcherRole(
            IBuiltTransactionRepository builtTransactionRepository)
        {
            _builtTransactionRepository = builtTransactionRepository;
        }


        public async Task<string> GetFromAddressAsync(Guid operationId)
        {
            return (await _builtTransactionRepository.GetAsync(operationId)).FromAddress;
        }
    }
}
