using UnityEngine;

using UndergroundFortress.Gameplay.Character;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Static data/Character/Enemy")]
    public class EnemyStaticData : CharacterStaticData
    {
        [Space]
        public int id;
        public EnemyData enemyData;
    }
}