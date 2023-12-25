using UnityEngine;

namespace UndergroundFortress.Scripts.Core.Services.Factories.Gameplay
{
    public interface IGameplayFactory : IService
    {
        public GameObject CreateMatch3();
    }
}