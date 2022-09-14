using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Core.Environments;


public static class ServiceLocator
{
    private static Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

    public static T GetService<T>() where T: class,IService
    {
        IService ret = _services.GetValueOrDefault(typeof(T), null);
        if(ret != null)
        {
            return (T)ret;
        }
        else
        {
            Debug.LogError("Trying to get a service that has not been added to the Service Locator");
            return (T)ret;
        }
    }

    public static void AddService<T>(IService service)
    {
        Type type = typeof(T);
        if(!_services.ContainsKey(type))
        {
            _services.Add(type, service);
        }
        else
        {
            Debug.LogError("Trying to add an already existing service to the Service Locator");
        }
    }
}