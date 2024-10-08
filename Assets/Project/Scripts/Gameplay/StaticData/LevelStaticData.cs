using UnityEngine;

using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.UI.Hud;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public DungeonBackground dungeonBackgroundPrefab;
        public HudView hudViewPrefab;

        [Space]
        public Canvas gameplayCanvas;
        public AttackArea attackArea;
        public PlayerData player;
        public DropItemView dropItemPrefab;
        public DamageValueView damageValuePrefab;
    }
}