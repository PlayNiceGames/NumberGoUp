using System;

namespace ServiceLocator
{
    public class MissingServiceException : Exception
    {
        public override string Message => $"Missing service: {_serviceType}";

        private readonly Type _serviceType;

        public MissingServiceException(Type serviceType)
        {
            _serviceType = serviceType;
        }
    }
}