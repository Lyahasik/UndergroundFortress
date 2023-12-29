using UnityEngine;

using UndergroundFortress.Scripts.Gameplay.Character;

using UndergroundFortress.Scripts.UI.Hud;

namespace UndergroundFortress.Scripts.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public HudView hudViewPrefab;

        public Canvas gameplayCanvas;
        public AttackArea attackArea;
    }
}