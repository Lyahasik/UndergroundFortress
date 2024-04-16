using System;
using UnityEngine;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.UI.Hud;

namespace UndergroundFortress.Gameplay.Dungeons.Services
{
    public interface IProgressDungeonService : IService
    {
        public event Action<bool, bool> OnEndLevel;

        public void Initialize(Canvas gameplayCanvas,
            DungeonBackground dungeonBackground,
            HudView hudView,
            PlayerData playerData,
            int dungeonId,
            int levelId);
        public void StartBattle();
        public void NextLevel();
    }
}