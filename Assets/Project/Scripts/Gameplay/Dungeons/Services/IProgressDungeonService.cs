using System;
using UnityEngine;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.UI.Hud;

namespace UndergroundFortress.Gameplay.Dungeons.Services
{
    public interface IProgressDungeonService : IService
    {
        public event Action OnSuccessLevel;

        public void Initialize(Canvas gameplayCanvas,
            HudView hudView,
            PlayerData playerData,
            int dungeonId,
            int levelId);
        public void StartBattle();
        public void NextLevel();
    }
}