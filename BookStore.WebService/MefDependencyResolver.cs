using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http.Dependencies;
using Microsoft.Practices.ServiceLocation;

namespace BookStore.WebService
{
    public class MefDependencyResolver : IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        private readonly IServiceLocator _serviceLocator;

        public MefDependencyResolver(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public void Dispose()
        {
            //
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            try
            {
                var instance = _serviceLocator.GetInstance(serviceType);
                return instance;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
            }

            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            try
            {
                IEnumerable<object> instances = _serviceLocator.GetAllInstances(serviceType);
                return instances;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
            }

            return new object[] { };
        }
    }
}