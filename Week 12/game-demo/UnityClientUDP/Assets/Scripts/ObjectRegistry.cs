using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public delegate NetworkObject SpawnDelegate();
static public class ObjectRegistry
{
    static private Dictionary<string, Type> registeredTypes = new Dictionary<string, Type>();
    
    static public void RegisterAll()
    {
        RegisterClass<Pawn>();
    }

    static public void RegisterClass<T>() where T : NetworkObject
    {
        string classID = (string)typeof(T).GetField("classID").GetValue(null);
        registeredTypes.Add(classID, typeof(T));
    }

    static public NetworkObject SpawnFrom(string classID)
    {
        if(registeredTypes.ContainsKey(classID))
        {
            return (NetworkObject)registeredTypes[classID].GetConstructor(new Type[] { }).Invoke(null);
        }
        return null;
    }
}
