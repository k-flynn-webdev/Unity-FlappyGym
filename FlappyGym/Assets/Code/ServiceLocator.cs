using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object>
        Services = new Dictionary<Type, object>();

    public static void Register<T>(object serviceInstance)
    {
        Services[typeof(T)] = serviceInstance;
    }

    public static T Resolve<T>()
    {
        return (T)Services[typeof(T)];
    }

    public static void Reset()
    {
        Services.Clear();
    }

    //public static void PrintServices()
    //{
    //    foreach (KeyValuePair<Type, object> pair in Services)
    //    {
    //        Debug.Log(pair.Key);
    //        Debug.Log(pair.Value);
    //        //Use pair.Key to get the key
    //        //Use pair.Value for value
    //    }
    //}
}
