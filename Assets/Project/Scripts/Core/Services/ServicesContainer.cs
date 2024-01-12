using System;
using System.Collections.Generic;
using UnityEngine;

namespace UndergroundFortress.Core.Services
{
    public class ServicesContainer
    {
        private Dictionary<Type, IService> _services = new ();

        public void Register<TService>(TService service) where TService : IService
        {
            _services.Add(typeof(TService), service);
        }

        public TService Single<TService>() where TService : class, IService
        {
            if (_services.TryGetValue(typeof(TService), out IService service))
                return service as TService;
            
            Debug.Log($"The requested service { typeof(TService) } is not available.");
            
            return null;
        }

        public void Clear()
        {
            _services.Clear();
        }
    }
}