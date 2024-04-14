using UnityEngine;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Character;

namespace UndergroundFortress.Gameplay.Dungeons.Services
{
    public interface IProgressDungeonService : IService
    {
        void Initialize(Canvas gameplayCanvas, PlayerData playerData);
        void StartBattle();
    }
}