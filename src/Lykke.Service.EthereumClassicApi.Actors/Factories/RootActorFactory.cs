﻿using Akka.Actor;
using Akka.DI.Core;
using Lykke.Service.EthereumClassicApi.Actors.Factories.Interfaces;

namespace Lykke.Service.EthereumClassicApi.Actors.Factories
{
    public class RootActorFactory : IRootActorFactory
    {
        private readonly ActorSystem _actorSystem;


        public RootActorFactory(
            ActorSystem actorSystem)
        {
            _actorSystem = actorSystem;
        }


        public IActorRef Build<T>(string name)
            where T : ActorBase
        {
            return _actorSystem.ActorOf
            (
                _actorSystem.DI().Props<T>(),
                name
            );
        }
    }
}
