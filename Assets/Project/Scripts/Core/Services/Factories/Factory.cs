using UnityEngine;

namespace UndergroundFortress.Core.Services.Factories
{
    public abstract class Factory
    {
        protected T PrefabInstantiate<T>(T prefab) where T : Behaviour
        {
            T newObject = Object.Instantiate(prefab);
            newObject.name = prefab.name;
            
            return newObject;
        }

        protected T PrefabInstantiate<T>(T prefab, Transform parent) where T : Behaviour
        {
            T newObject = Object.Instantiate(prefab, parent);
            newObject.name = prefab.name;
            
            return newObject;
        }
    }
}