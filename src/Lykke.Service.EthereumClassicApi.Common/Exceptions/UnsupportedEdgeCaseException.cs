﻿using System;

namespace Lykke.Service.EthereumClassicApi.Common.Exceptions
{
    public class UnsupportedEdgeCaseException : Exception
    {
        public UnsupportedEdgeCaseException()
        {
        }

        public UnsupportedEdgeCaseException(string message)
            : base(message)
        {
        }

        public UnsupportedEdgeCaseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
