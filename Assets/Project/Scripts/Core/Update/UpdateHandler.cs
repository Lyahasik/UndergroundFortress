using System.Collections.Generic;
using UnityEngine;

namespace UndergroundFortress.Core.Update
{
    public class UpdateHandler : MonoBehaviour
    {
        private List<IUpdating> _updatedObjects;

        private void Awake()
        {
            _updatedObjects = new List<IUpdating>();
        }

        public void Update()
        {
            _updatedObjects.ForEach(data => data.Update());
        }

        public void AddUpdatedObject(IUpdating updating) => 
            _updatedObjects.Add(updating);

        public void RemoveUpdatedObject(IUpdating updating) => 
            _updatedObjects.Remove(updating);
    }
}